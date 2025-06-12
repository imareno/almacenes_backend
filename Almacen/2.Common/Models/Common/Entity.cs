namespace Api._2.Common.Models.Generic
{
    public class Entity
    {
        public Guid Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public int? UsuarioCreacion { get; set; }
        public Guid? Token { get; set; }
    }
}
