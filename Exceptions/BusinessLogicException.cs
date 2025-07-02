using Hotel_Management.Models.Enums;

namespace Hotel_Management.Exceptions
{
    public class BusinessLogicException:BaseApplicationException
    {
        public BusinessLogicException(string message, ErrorCode errorCode)
           : base(message, errorCode, StatusCodes.Status400BadRequest)
        {
        }

        public BusinessLogicException(string message, ErrorCode errorCode, Exception innerException)
            : base(message, errorCode, StatusCodes.Status400BadRequest, innerException)
        {
        }
    }
}
