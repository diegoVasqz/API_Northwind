using API_Northwind.Data;
using API_Northwind.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace API_Northwind.Controllers
{
    [ApiController]
    [Route("Northwind_API")]
    public class RegionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private DataMySQL _data_region;

        #region Name_StoredProcedures
        public readonly string _getRegionList = "GETRegionList";
        public readonly string _Ups_Ins_Region = "UPS_INS_REGION";
        public readonly string _Ups_Upd_Region = "UPS_UPD_REGION";
        public readonly string _Ups_Del_Region = "UPS_DEL_REGION";
        #endregion

        public RegionController(IConfiguration configuration)
        {
            _configuration = configuration;
            _data_region = new DataMySQL(_configuration);
        }


        [HttpGet]
        [Route("ListRegion")]
        public object GetRegionList()
        {
            try
            {
                _data_region = new DataMySQL(_configuration);
                DataTable dtData = _data_region.ExecuteGet(_getRegionList);

                if (dtData.Rows != null)
                {
                    string ResultList = JsonConvert.SerializeObject(dtData);

                    var JsonResult = JsonConvert.DeserializeObject<List<ENT_Region>>(ResultList);
                    var ObjectResult = new
                    {
                        succes = true,
                        message = "OK",
                        result = new
                        {
                            Region = JsonResult
                        }
                    };

                    return ObjectResult;
                }

                else
                {
                    return new
                    {
                        succes = false,
                        message = "Error al consumir al consultar la base de datos",
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
        [Route("InsRegion")]
        public object Ins_Region([FromBody] List<ENT_Region> ent_region)
        {
            try
            {
                List<ENT_Parameter> parametros = new List<ENT_Parameter>();

                foreach (var _entReg in ent_region)
                {
                    parametros.Add(new ENT_Parameter("@p_RegionId", _entReg.RegionID));
                    parametros.Add(new ENT_Parameter("@p_RegionDescription", _entReg.RegionDescription));
                }

                dynamic res = _data_region.ExecuteStoredProcedure(_Ups_Ins_Region, parametros);

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
        [Route("UpdRegion")]
        public object Upd_Region([FromBody] List<ENT_Region> ent_region)
        {
            try
            {
                List<ENT_Parameter> parametros = new List<ENT_Parameter>();

                foreach (var _entReg in ent_region)
                {
                    parametros.Add(new ENT_Parameter("@p_RegionId", _entReg.RegionID));
                    parametros.Add(new ENT_Parameter("@p_RegionDescription", _entReg.RegionDescription));
                }

                dynamic res = _data_region.ExecuteStoredProcedure(_Ups_Upd_Region, parametros);

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
        [Route("DelRegion/{id}")]
        public object Delete(string id)
        {
            try
            {
                dynamic res = _data_region.ExecuteParameter(_Ups_Del_Region, id);

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
