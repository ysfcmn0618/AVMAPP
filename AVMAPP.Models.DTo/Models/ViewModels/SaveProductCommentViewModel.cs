﻿using System.ComponentModel.DataAnnotations;

namespace AVMAPP.Models.DTo.Models.ViewModels
{
    public class SaveProductCommentViewModel
    {
        [Required, MinLength(5), MaxLength(500)]
        public string Text { get; set; } = null!;

        [Required, Range(1, 5)]
        public byte StarCount { get; set; }
    }
}