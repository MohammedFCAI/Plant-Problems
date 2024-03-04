namespace Plant_Problems.Core.Features.Authentications.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class GmailAddressAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            string email = value.ToString();
            return email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase);
        }
    }
}
