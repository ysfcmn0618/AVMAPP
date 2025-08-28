using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    public class DiscountEntity : GenericField
    {
        public byte DiscountRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Enabled { get; set; } = true;
    }

    internal class DiscountEntityConfiguration : IEntityTypeConfiguration<DiscountEntity>
    {
        public void Configure(EntityTypeBuilder<DiscountEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Enabled).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).IsRequired();           
        }
    }

    
}
