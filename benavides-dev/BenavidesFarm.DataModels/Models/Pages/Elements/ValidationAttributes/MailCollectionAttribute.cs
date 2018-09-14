using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages.Elements.ValidationAttributes
{
    class MailCollectionAttribute: ValidationAttribute
    {
        private string errorMessagePlaceHolder;
        private bool invalid;
       
        public MailCollectionAttribute(string errorMessagePlaceHolder)
        {
            this.errorMessagePlaceHolder = errorMessagePlaceHolder;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                string mailCollection = value as string;

                IEnumerable<string> mails = mailCollection.Split(';');

                IEnumerable<string> incorrectMails = mails.Where(m => this.IsValidEmail(m) == false)
                                                    .Select(m => String.Format(this.errorMessagePlaceHolder, m));
                var errorMessage = String.Join(",\n", incorrectMails);

                if (errorMessage.Count() > 0)
                {
                    return new ValidationResult(errorMessage);
                }
            }       


            return ValidationResult.Success;
        }

        protected bool IsValidEmail(string mail)
        {
            invalid = false;
            mail = mail.Trim();

            if (String.IsNullOrEmpty(mail))
                return false;           

            try
            {
                mail = Regex.Replace(mail, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            try
            {
                return Regex.IsMatch(mail,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        protected string DomainMapper(Match match)
        {
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }



    }
}
