namespace MintSerivce.Models
{
    public class PDFFileReturnModel
    {
        public bool IsSuccess { get; set; }

        public byte[] FileContent { get; set; }

        public string ResultMessage { get; set; }
    }
}