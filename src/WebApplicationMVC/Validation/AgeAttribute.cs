using System.ComponentModel.DataAnnotations;

namespace Store.WebApplicationMVC.Validation
{
    public class AgeAttribute : ValidationAttribute
    {
        public AgeAttribute(int age)
        {
            Age = age;
        }
        public int Age { get; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            DateTime dateOfBirth = DateTime.UtcNow;

            if(value is DateOnly date)
            {
                dateOfBirth = date.ToDateTime(TimeOnly.MinValue);
            }
            else
            {
                dateOfBirth = (DateTime)value;
            }

            return (dateOfBirth.AddYears(Age) <= DateTime.Now) ?
                ValidationResult.Success :
                new ValidationResult($"User is NOT old enough. Expected age at least: {Age}");
        }
    }
}
