﻿namespace MintSerivce.Models
{
    public class SimActivateResponseDto
    {
        public string VerserOrderID { get; set; }
        public bool IsActivated { get; set; }
        public string Message { get; set; }
        public string DateActivated { get; set; }
    }
}