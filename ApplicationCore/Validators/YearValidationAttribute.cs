using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Validators
{
    public class YearValidationAttribute : ValidationAttribute
    {
        public int MinYear { get;}
        public int MaxYear { get; }
        public YearValidationAttribute(int year)
        {
            MinYear = year;
            MaxYear = Convert.ToInt32(DateTime.Today.Year);
        }

        public YearValidationAttribute(int minYear, int maxYear)
        {
            MinYear = minYear;
            MaxYear = maxYear;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var userEnterYear = ((DateTime)value).Year;

            if (userEnterYear < MinYear || userEnterYear > MaxYear)
            {
                return new ValidationResult("Please enter correct year");
            }

            return base.IsValid(value, validationContext);
        }
    }
}
