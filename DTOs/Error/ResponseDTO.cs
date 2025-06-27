using Hotel_Management.Models.Enums;

namespace Hotel_Management.DTOs.Error
{
    public class ResponseDTO<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ErrorCode errorCode { get; set; }
    }
}
