using Hotel_Management.Models.Enums;

namespace Hotel_Management.Exceptions
{
    public class NotFoundException:BaseApplicationException
    {
        public NotFoundException(string message, ErrorCode errorCode)
           : base(message, errorCode, StatusCodes.Status400BadRequest)
        {
        }

        public NotFoundException(string message, ErrorCode errorCode, Exception innerException)
            : base(message, errorCode, StatusCodes.Status400BadRequest, innerException)
        {
        }
    }
}
