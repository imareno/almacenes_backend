using Almacen._2.Common.Models.System;

namespace Api._1.API.Utils
{
    public class ApiUtils
    {
        public static ApiResponse<T> Response<T>(T data)
        {
            return new ApiResponse<T> { Data = data };
        }
    }
}
