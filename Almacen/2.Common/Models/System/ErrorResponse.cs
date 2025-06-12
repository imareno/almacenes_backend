using System.Text.Json;

namespace Almacen._2.Common.Models.System
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {

        }
        public ErrorResponse(string status, string tittle, string description)
        {

            Errors = new List<string> { description };
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this).ToLower();
        }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
