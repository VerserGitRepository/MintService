namespace MintSerivce.Models
{
    public class OrderReplacementReturnDto
    {
        public string VerserOrderID { get; set; }
        public string VerserReturnOrderID { get; set; }
        public string TIABOrderID { get; set; }
        public string OrderStatus { get; set; }
        public string ErrorMessage { get; set; }
    }
}