namespace Almacen._1.Api.DTO
{
    public class UserDto
    {
        public int id { get; set; }
        public string persona_id { get; set; }
        public DatosPersonalesDto persona { get; set; }
    }

    public class DatosPersonalesDto : DatosUsuarioBase
    {
        public int id { get; set; }
        public string fotografia { get; set; }
    }

    public class DatosUsuarioBase
    {
        public string nombre_completo { get; set; }
        public string ci { get; set; }
        public string organigrama { get; set; }
        public string cargo { get; set; }
        public string? tipo_cargo { get; set; }
    }
}
