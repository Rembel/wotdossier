using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace WotDossier.Framework.Validation
{
    public class PathExistsValidationRule : ValidationRule
    {
        public string ErrorMessageFormat { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = ValidationResult.ValidResult;

            var path = value as string ?? string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                if (!Directory.Exists(path))
                {
                    result = new ValidationResult(false, string.Format(ErrorMessageFormat, path));
                }
            }

            return result; 
        }
    }
}
