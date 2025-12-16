using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Weather_Vinokurov.Classes
{
    public class CityValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string city = value as string;

            if (string.IsNullOrWhiteSpace(city))
            {
                return new ValidationResult(false, "Введите название города");
            }

            if (city.Length < 2)
            {
                return new ValidationResult(false, "Название города слишком короткое");
            }

            return ValidationResult.ValidResult;
        }
    }
}
