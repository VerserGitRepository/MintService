using System;
using System.ComponentModel.DataAnnotations;

namespace MintSerivce.Models
{
    public class FilesModel
    {
        public string FileName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime FileDate { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public string FileFullPath { get; set; }
    }
}