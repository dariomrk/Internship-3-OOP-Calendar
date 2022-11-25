using System.Net.Mail;

namespace Internship_3_OOP_Calendar.Classes
{
    /// <summary>
    /// Contains <see cref="Validate(string)"/> static method.
    /// </summary>
    public static class Email
    {
        /// <summary>
        /// Check Email validity.
        /// </summary>
        /// <param name="email">Email to check.</param>
        /// <returns>Boolean indicating wether the email is valid.</returns>
        public static bool Validate(string email)
        {
            try
            {
                MailAddress m = new(email);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
