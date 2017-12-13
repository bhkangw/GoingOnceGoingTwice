using System;
using System.ComponentModel.DataAnnotations;

namespace csharpbelt.Models
{
    public class AddBid : BaseEntity
    {
        [Display(Name = "Bid Amount")]
        [Required]
        [Range(0.01,9999)]
        public double Amount { get; set; }

    }

    public class HighestBidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return d >= DateTime.Now;
        }
    }
}