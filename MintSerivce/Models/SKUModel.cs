using System.ComponentModel.DataAnnotations;

namespace MintSerivce.Models
{
    public class SKUModel
    {
        [Required(ErrorMessage = "SKU is required")]
        public string SKU { get; set; }
        [Required(ErrorMessage = "Make is required")]
        public string Make { get; set; }
        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Capacity is required")]
        public string Capacity { get; set; }
        [Required(ErrorMessage = "Colour is required")]
        public string Colour { get; set; }
        [Required(ErrorMessage = "SubGrade is required")]
        public string SubGrade { get; set; }
        [Required(ErrorMessage = "SKUBuffer is required")]
        public string SKUBuffer { get; set; }
        [Required(ErrorMessage = "IsActive is required")]
        public bool IsActive { get; set; }
        public string EditingSKU { get; set; }
    }
}