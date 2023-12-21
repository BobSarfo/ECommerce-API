namespace HubtelCommerce.Utils
{
    public static class StringUtils
    {
        public static string ConvertToLocalFormat(this string phoneNumber)
        {
            // Remove any spaces or non-digit characters
            phoneNumber = new string(phoneNumber.Trim().Where(char.IsDigit).ToArray());

            // Check if the number starts with "+233" or "233"
            if (phoneNumber.StartsWith("+233"))
            {
                phoneNumber = "0" + phoneNumber.Substring(4);
            }
            else if (phoneNumber.StartsWith("233"))
            {
                phoneNumber = "0" + phoneNumber.Substring(3);
            }

            // Trim or pad to ensure the total number of digits is 10
            if (phoneNumber.Length > 10)
            {
                phoneNumber = phoneNumber.Substring(0, 10);
            }
            else if (phoneNumber.Length < 10)
            {
                phoneNumber = phoneNumber.PadRight(10, '0');
            }

            return phoneNumber;
        }
    }
}
