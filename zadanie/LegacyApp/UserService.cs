using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateUserInput(firstName, lastName, email, dateOfBirth))
                return false;
            
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = CreateUserObject(firstName, lastName, email, dateOfBirth, client);
            SetCreditDetails(user, client);

            
            if (CheckUserSettingsAboutCredit(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private void SetCreditDetails(User user, Client client)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }
        }

        private bool ValidateUserInput(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if(IsFirstNameAndLastNameNull(firstName, lastName) || IsEmailInvalid(email) || IsUnderCertainAge(dateOfBirth))
                return false;

            return true;
        }
        
        private User CreateUserObject(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
        }
        
        private bool CheckUserSettingsAboutCredit(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }

        private bool IsFirstNameAndLastNameNull(string firstName, string lastName)
        {
            return string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName);
        }
        
        private bool IsEmailInvalid(string email)
        {
            return !email.Contains("@") && !email.Contains(".");
        }

        private bool IsUnderCertainAge(DateTime dateOfBirth)
        {
            bool isUnderCertainAge = false;
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                isUnderCertainAge = true;
            }
            
            return isUnderCertainAge;
        }
    }
}
