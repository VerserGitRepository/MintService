namespace MintSerivce.Models
{
    public class CancelOrderModel
    {
        //  public int ID { get; set; }
        public string VerserOrderID { get; set; }
        public string TIABOrderID { get; set; }
        public string OrderStatus { get; set; }
        public string ErrorMessage { get; set; }


    }
}