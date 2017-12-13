using System;
using System.ComponentModel.DataAnnotations;

namespace csharpbelt.Models
{
    public class AddAuction : BaseEntity
    {
        [Display(Name = "Product Name")]
        [Required]
        [MinLength(3)]
        public string ProductName { get; set; }
        
        [Display(Name = "Description")]
        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Display(Name = "Starting Bid")]
        [Required]
        [Range(.01,9999)]
        public double StartingBid { get; set; }

        [Display(Name = "End Date")]
        [Required]
        [MyDate(ErrorMessage = "Date must be in the future")]
        public DateTime EndDate { get; set; }

    }

    public class MyDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return d >= DateTime.Now;
        }
    }
}