namespace PhotoShare.Models.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    internal class EmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var email = value as string;

            return !string.IsNullOrEmpty(email) && email.Contains("@");
        }
    }
}
