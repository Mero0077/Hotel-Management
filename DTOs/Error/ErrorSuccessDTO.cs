using Hotel_Management.Models.Enums;

namespace Hotel_Management.DTOs.Error
{
    public class ErrorSuccessDTO<T>:ResponseDTO<T>
    {
        public ErrorSuccessDTO(T Data, string message = "")
        {
            this.Data = Data;
            Message = message;
            IsSuccess = true;
            errorCode = ErrorCode.NoError;
        }
    }
}
