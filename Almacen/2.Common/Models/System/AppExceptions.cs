namespace Almacen._2.Common.Models.System;


[Serializable]
public class AppBadRequestException : Exception
{
    public AppBadRequestException(string message) : base(message)
    {
    }
}

public class AppNotFoundException : Exception
{
    public AppNotFoundException(string message) : base(message)
    {
    }
}

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}

public class ServicesException : Exception
{
    public int StatusCode { get; }
    public ServicesException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public ServicesException(string message, int statusCode, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }

}
