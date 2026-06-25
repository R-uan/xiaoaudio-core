namespace AudioArchive.Shared
{
  public class APIException(string Message, int StatusCode) : Exception(Message)
  {
    public int StatusCode { get; set; } = StatusCode;
  }

  public class NotFoundException(string Message, string Target) :
    APIException(Message, StatusCodes.Status404NotFound)
  {
    public string Target { get; } = Target;
  }

  public class DuplicatedException(string Message, string Target) :
     APIException(Message, StatusCodes.Status409Conflict)
  {
    public string Target { get; } = Target;
  }

  public class BadRequestException(string Message, string Target)
    : APIException(Message, StatusCodes.Status400BadRequest)
  {
    public string Target { get; } = Target;
  }

  public class VerificationException(string Message, string Target)
    : APIException(Message, StatusCodes.Status400BadRequest)
  {
    public string Target { get; } = Target;
  }

  public class SupportException(string Message, string Target)
    : APIException(Message, StatusCodes.Status400BadRequest)
  {
    public string Target { get; } = Target;
  }

  public class UnauthorizedException(string Message, string Target)
    : APIException(Message, StatusCodes.Status401Unauthorized)
  {
    public string Target { get; } = Target;
  }

  public class ReservedException(string Message, string Target)
    : APIException(Message, StatusCodes.Status400BadRequest)
  {
    public string Target { get; } = Target;
  }

}
