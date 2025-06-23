using Hotel_Management.Models.Enums;

namespace Hotel_Management.Models.ViewModels.Errors
{
    public class SuccessResponseVM<T> : ResponseVM<T>
    {
        public SuccessResponseVM(T Data, string message = "")
        {
            this.Data = Data;
            Message = message;
            IsSuccess = true;
            errorCode = ErrorCode.NoError;
        }
    }
}
