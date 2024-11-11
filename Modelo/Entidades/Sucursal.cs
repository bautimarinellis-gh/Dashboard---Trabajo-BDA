using Modelo.Entidades;

public class Sucursal
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Ubicacion { get; set; }
    public string Telefono { get; set; }
    public List<Auto> Autos { get; set; }
    public List<Venta> Ventas { get; set; }

    // Monto total es la suma del precio final de todas las ventas
    public decimal MontoTotal
    {
        get
        {
            return Ventas?.Sum(v => v.PrecioFinal) ?? 0;  // Suma todas las ventas o retorna 0 si no hay ventas
        }
    }

    // Estado basado en el MontoTotal
    public string Estado
    {
        get
        {
            if (MontoTotal < 100000)
                return "Rojo";
            else if (MontoTotal >= 100000&& MontoTotal < 150000)
                return "Amarillo";
            else
                return "Verde";
        }
    }

    public Sucursal()
    {
        Autos = new List<Auto>();
        Ventas = new List<Venta>();
    }
}
