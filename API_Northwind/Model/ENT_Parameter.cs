namespace API_Northwind.Model
{
    public class ENT_Parameter
    {
        public ENT_Parameter(string nombre, dynamic valor)
        {
            Nombre = nombre;
            Valor = valor;
        }

        public string Nombre { get; set; }
        public dynamic Valor { get; set; }
    }
}
