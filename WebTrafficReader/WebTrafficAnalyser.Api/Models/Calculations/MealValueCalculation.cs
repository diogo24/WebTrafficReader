using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTrafficAnalyser.Api.Models.Calculations
{
    public class MealValueCalculation
    {
        [Required]
        public decimal BaseValue { get; set; }
        [Required]
        public int NumberRepetitions { get; set; }
        [Required]
        public int ExpectedValue { get; set; }
    }
}
