﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Product
{
    public class MyProductsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string Description { get; set; } = default!;
        public byte Stock { get; set; }
        public bool HasDiscount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
