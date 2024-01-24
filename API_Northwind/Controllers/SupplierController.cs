using API_Northwind.Data;
using API_Northwind.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;

namespace API_Northwind.Controllers
{
    [ApiController]
    [Route("Northwind_API")]
    public class SupplierController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private DataMySQL _data_supplier;

        #region Name_StoredProcedures
        public readonly string _getRegionList = "GETRegionList";
        public readonly string _Ups_Ins_Supplier = "UPS_INS_SUPPLIER";
        public readonly string _Ups_Upd_Supplier = "UPS_UPD_SUPPLIER";
        public readonly string _Ups_Del_Supplier = "UPS_DEL_SUPPLIER";
        #endregion

        public SupplierController(IConfiguration configuration)
        {
            _configuration = configuration;
            _data_supplier = new DataMySQL(_configuration);
        }


        [HttpGet]
        [Route("ListRegion")]
        public object GetRegionList()
        {
            try {

                _data_supplier = new DataMySQL(_configuration);
                DataTable dtData = _data_supplier.ExecuteGet(_getRegionList);

                if (dtData.Rows != null)
                {
                    string ResultList = JsonConvert.SerializeObject(dtData);

                    var JsonResult = JsonConvert.DeserializeObject<List<ENT_Supplier>>(ResultList);
                    var ObjectResult = new
                    {
                        succes = true,
                        message = "OK",
                        result = new
                        {
                            Suppliers = JsonResult
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
            catch (Exception ex){

                return new
                {
                    succes = false,
                    message = "Error inesperado: " + ex.Message,
                };
            }

            
        }


        [HttpPost]
        [Route("InsSupplier")]
        public object Ins_Supplier([FromBody] List<ENT_Supplier> ent_supplier)
        {
            try
            {
                List<ENT_Parameter> parametros = new List<ENT_Parameter>();

                foreach (var _entSup in ent_supplier)
                {
                    parametros.Add(new ENT_Parameter("@p_CompanyName", _entSup.CompanyName));
                    parametros.Add(new ENT_Parameter("@p_ContactName", _entSup.ContactName));
                    parametros.Add(new ENT_Parameter("@p_ContactTitle", _entSup.ContactTitle));
                    parametros.Add(new ENT_Parameter("@p_Address", _entSup.Address));
                    parametros.Add(new ENT_Parameter("@p_City", _entSup.City));
                    parametros.Add(new ENT_Parameter("@p_Region", _entSup.Region));
                    parametros.Add(new ENT_Parameter("@p_PostalCode", _entSup.PostalCode));
                    parametros.Add(new ENT_Parameter("@p_Country", _entSup.Country));
                    parametros.Add(new ENT_Parameter("@p_Phone", _entSup.Phone));
                    parametros.Add(new ENT_Parameter("@p_Fax", _entSup.Fax));
                    parametros.Add(new ENT_Parameter("@p_HomePage", _entSup.HomePage));
                }

                dynamic res = _data_supplier.ExecuteStoredProcedure(_Ups_Ins_Supplier, parametros);

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
        [Route("UpdSupplier")]
        public object Upd_Supplier([FromBody] List<ENT_Supplier> ent_supplier)
        {
            try
            {
                List<ENT_Parameter> parametros = new List<ENT_Parameter>();

                foreach (var _entSup in ent_supplier)
                {
                    parametros.Add(new ENT_Parameter("@p_SupplierID", _entSup.SupplierID));
                    parametros.Add(new ENT_Parameter("@p_CompanyName", _entSup.CompanyName));
                    parametros.Add(new ENT_Parameter("@p_ContactName", _entSup.ContactName));
                    parametros.Add(new ENT_Parameter("@p_ContactTitle", _entSup.ContactTitle));
                    parametros.Add(new ENT_Parameter("@p_Address", _entSup.Address));
                    parametros.Add(new ENT_Parameter("@p_City", _entSup.City));
                    parametros.Add(new ENT_Parameter("@p_Region", _entSup.Region));
                    parametros.Add(new ENT_Parameter("@p_PostalCode", _entSup.PostalCode));
                    parametros.Add(new ENT_Parameter("@p_Country", _entSup.Country));
                    parametros.Add(new ENT_Parameter("@p_Phone", _entSup.Phone));
                    parametros.Add(new ENT_Parameter("@p_Fax", _entSup.Fax));
                    parametros.Add(new ENT_Parameter("@p_HomePage", _entSup.HomePage));
                }

                dynamic res = _data_supplier.ExecuteStoredProcedure(_Ups_Upd_Supplier, parametros);

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
        [Route("DelSupplier/{id}")]
        public dynamic Delete(string id)
        {
            try 
            {
                dynamic res = _data_supplier.ExecuteParameter(_Ups_Del_Supplier, id);

                return new
                {
                    success = res.Process,
                    message = res.Message,
                    result = ""
                };
            }
            catch(Exception ex) 
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
