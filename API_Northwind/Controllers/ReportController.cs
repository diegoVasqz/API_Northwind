using API_Northwind.Data;
using API_Northwind.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace API_Northwind.Controllers
{
    [ApiController]
    [Route("Northwind_API")]

    public class ReportController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private DataMySQL _data_order;

        #region Name_StoredProcedures
        public readonly string _getOrderList = "GETOrderList";
        public readonly string _getOrderDetailList = "GETOrderdetailList";
        public readonly string _getProductList = "GETProductList";
        public readonly string _getSupplierList = "GETSupplierList";
        #endregion

        public ReportController(IConfiguration configuration)
        {
            _configuration = configuration;
            _data_order = new DataMySQL(_configuration);
        }



        [HttpGet]
        [Route("ListReport")]
        public object GetOrderList()
        {
            try
            {
                _data_order = new DataMySQL(_configuration);
                DataTable dtData = _data_order.ExecuteGet(_getOrderList);

                if (dtData.Rows != null)
                {
                    string ResultList = JsonConvert.SerializeObject(dtData);

                    var JsonResult = JsonConvert.DeserializeObject<List<ENT_Orders>>(ResultList);


                    foreach (var order in JsonResult)
                    {
                        order.OrderDetails = GetOrderDetails(order.OrderID);
                    }


                    var ObjectResult = new
                    {
                        succes = true,
                        message = "OK",
                        result = new
                        {
                            Orders = JsonResult
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


        private List<ENT_Orderdetails> GetOrderDetails(int orderId)
        {
            try
            {
                _data_order = new DataMySQL(_configuration);
                DataTable dtData = _data_order.ExecuteGetId(_getOrderDetailList, orderId);

                if (dtData.Rows != null)
                {
                    string ResultList = JsonConvert.SerializeObject(dtData);

                    var JsonResult = JsonConvert.DeserializeObject<List<ENT_Orderdetails>>(ResultList);

                    foreach (var order in JsonResult)
                    {
                        order.Products = GetProducts(order.ProductID);
                    }

                    return JsonResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private List<ENT_Products> GetProducts(int Idproduct)
        {
            try
            {
                _data_order = new DataMySQL(_configuration);
                DataTable dtData = _data_order.ExecuteGetId(_getProductList, Idproduct);

                if (dtData.Rows != null)
                {
                    string ResultList = JsonConvert.SerializeObject(dtData);

                    var JsonResult = JsonConvert.DeserializeObject<List<ENT_Products>>(ResultList);

                    foreach (var order in JsonResult)
                    {
                        order.Suppliers = GetSuppliers(order.SupplierID);
                    }

                    return JsonResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private List<ENT_Supplier> GetSuppliers(int Idsupplier)
        {
            try
            {
                _data_order = new DataMySQL(_configuration);
                DataTable dtData = _data_order.ExecuteGetId(_getSupplierList, Idsupplier);

                if (dtData.Rows != null)
                {
                    string ResultList = JsonConvert.SerializeObject(dtData);

                    var JsonResult = JsonConvert.DeserializeObject<List<ENT_Supplier>>(ResultList);

                    return JsonResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
