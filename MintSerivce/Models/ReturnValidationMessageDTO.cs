namespace MintSerivce.Models
{
    public class ReturnValidationMessageDTO
    {
        public string ResultMessage { get; set; }
        public bool IsValidateState { get; set; }
        public bool RevertState { get; set; } = false;
    }
}