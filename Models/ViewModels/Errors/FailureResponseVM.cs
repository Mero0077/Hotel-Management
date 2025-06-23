using Hotel_Management.Models.Enums;

namespace Hotel_Management.Models.ViewModels.Errors
{
    public class FailureResponseVM<T> : ResponseVM<T>
    {
        public FailureResponseVM(ErrorCode errorCode, string message = "")
        {
            Message = message;
            IsSuccess = false;
            this.errorCode = errorCode;
        }
    }
}
