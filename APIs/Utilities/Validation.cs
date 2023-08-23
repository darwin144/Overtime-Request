using APIs.Contract;
using System.ComponentModel.DataAnnotations;

namespace APIs.Utilities
{
    public class Validation : ValidationAttribute
    {
        private readonly string _propertyName;
        public Validation(string propertyName)
        {
            _propertyName = propertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult($"{_propertyName} is required.");
            var employeeRepository = validationContext.GetService(typeof(IEmployeeRepository)) as IEmployeeRepository; //awalnya casting pengganti var

            var checkEmailAndPhone = employeeRepository.CheckValidation(value.ToString());
            if (checkEmailAndPhone) return new ValidationResult($"{_propertyName} '{value}' already exists.");
            return ValidationResult.Success;
        }

    }
}
