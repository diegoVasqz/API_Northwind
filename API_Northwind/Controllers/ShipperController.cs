using API_Northwind.Data;
using API_Northwind.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace API_Northwind.Controllers
{
    [ApiController]
    [Route("Northwind_API")]

    public class ShipperController : ControllerBase
    {


        private readonly IConfiguration _configuration;
        private DataMySQL _data_shipper;

        #region Name_StoredProcedures
        public readonly string _getShipperList = "GETShipperList";
        public readonly string _Ups_Ins_Shipper = "UPS_INS_SHIPPER";
        public readonly string _Ups_Upd_Shipper = "UPS_UPD_SHIPPER";
        public readonly string _Ups_Del_Shipper = "UPS_DEL_SHIPPER";
        #endregion


        public ShipperController(IConfiguration configuration)
        {
            _configuration = configuration;
            _data_shipper = new DataMySQL(_configuration);
        }


        [HttpGet]
        [Route("ListShipper")]
        public object GetShipperList()
        {
            try
            {
                _data_shipper = new DataMySQL(_configuration);
                DataTable dtData = _data_shipper.ExecuteGet(_getShipperList);

                if (dtData.Rows != null)
                {
                    string ResultList = JsonConvert.SerializeObject(dtData);

                    var JsonResult = JsonConvert.DeserializeObject<List<ENT_Shipper>>(ResultList);
                    var ObjectResult = new
                    {
                        succes = true,
                        message = "OK",
                        result = new
                        {
                            Shippers = JsonResult
                        }
                    };

                    return ObjectResult;
                }

                else
                {
                    return new
                    {
                        succes = false,
                        message = "Error al acceder a la base de datos",
                    };
                }
            }
            catch (Exception ex)
            {

                return new
                {
                    succes = false,
                    message = "Error inesperado: " + ex.Message,
                };
            }


        }



        [HttpPost]
        [Route("InsShipper")]
        public object Ins_Shipper([FromBody] List<ENT_Shipper> ent_shipper)
        {
            try
            {
                List<ENT_Parameter> parametros = new List<ENT_Parameter>();

                foreach (var _entShip in ent_shipper)
                {
                    parametros.Add(new ENT_Parameter("@p_CompanyName", _entShip.CompanyName));
                    parametros.Add(new ENT_Parameter("@p_Phone", _entShip.Phone));
                }

                dynamic res = _data_shipper.ExecuteStoredProcedure(_Ups_Ins_Shipper, parametros);

                return new
                {
                    success = res.Process,
                    message = res.Message,
                    result = ""
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Error inesperado: " + ex.Message
                };

            }

        }



        [HttpPost]
        [Route("UpdShipper")]
        public object Upd_Shipper([FromBody] List<ENT_Shipper> ent_shipper)
        {
            try
            {
                List<ENT_Parameter> parametros = new List<ENT_Parameter>();

                foreach (var _entShip in ent_shipper)
                {
                    parametros.Add(new ENT_Parameter("@p_ShipperId", _entShip.ShipperID));
                    parametros.Add(new ENT_Parameter("@p_CompanyName", _entShip.CompanyName));
                    parametros.Add(new ENT_Parameter("@p_Phone", _entShip.Phone));
                }

                dynamic res = _data_shipper.ExecuteStoredProcedure(_Ups_Upd_Shipper, parametros);

                return new
                {
                    success = res.Process,
                    message = res.Message,
                    result = ""
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Error inesperado: " + ex.Message
                };

            }


        }



        [HttpPost]
        [Route("DelShipper/{id}")]
        public object Delete(string id)
        {
            try
            {
                dynamic res = _data_shipper.ExecuteParameter(_Ups_Del_Shipper, id);

                return new
                {
                    success = res.Process,
                    message = res.Message,
                    result = ""
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message
                };
            }

        }

    }
}
