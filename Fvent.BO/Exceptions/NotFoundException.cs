namespace Fvent.BO.Exceptions;

public class NotFoundException(Type type) : Exception($"Not found {type.Name}")
{
}

