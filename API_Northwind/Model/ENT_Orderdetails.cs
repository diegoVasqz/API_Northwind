namespace API_Northwind.Model
{
    public class ENT_Orderdetails
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public List<ENT_Products> Products { get; set; }
        public decimal UnitPrice { get; set; } 
        public int Quantity { get; set; }
        public float Discount { get; set; }
    }
}
