using Hotel_Management.Models.Enums;

namespace Hotel_Management.DTOs.Error
{
    public class ErrorFailDTO<T> : ResponseDTO<T>
    {
        public ErrorFailDTO(ErrorCode errorCode, string message = "")
        {
            Message = message;
            IsSuccess = false;
            this.errorCode = errorCode;
        }
    }
}
