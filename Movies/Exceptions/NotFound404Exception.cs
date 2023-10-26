namespace Movies.Exceptions;

public class NotFound404Exception: Exception
{
    public NotFound404Exception(string message) : base(message)
    {
    }
}