using System;
using System.Collections.Generic;
using System.Text;

namespace BankOCRKata
{
    class AccountResolvingService
    {
        public Account RepairedAccount(Account faultyAccount)
        {
            var alternateNumbers = new List<string>();
            for (int i = 0; i < faultyAccount.Digits.Count; i++)
            {
                alternateNumbers.AddRange(GetAlternateAccountNumbers(faultyAccount, i));
            }
            if (alternateNumbers.Count.Equals(0))
                return faultyAccount;
            else if (alternateNumbers.Count.Equals(1))
                return new Account
                {
                    AccountNumber = alternateNumbers[0],
                    CheckSum = 0,
                    StatusCode = string.Empty
                };
            else
                return new Account
                {
                    AccountNumber = faultyAccount.AccountNumber,
                    CheckSum = -1,
                    StatusCode = "AMB"
                };
        }

        private List<string> GetAlternateAccountNumbers(Account faultyAccount, int digitToConvert)
        {
            var returnValue = new List<string>();
            var digit = faultyAccount.Digits[digitToConvert];
            var updatedAccount = string.Empty;
            for (int i = 0; i < digit.Characters.Length; i++)
            {
                var characters = digit.Characters;
                var currentCharacter = characters.Substring(i, 1);
                if (string.IsNullOrWhiteSpace(currentCharacter))
                {
                    updatedAccount = GetUpdatedAccountNumber(characters, i, '_', faultyAccount.AccountNumber, digitToConvert);
                    if (!string.IsNullOrEmpty(updatedAccount)) returnValue.Add(updatedAccount);
                    updatedAccount = GetUpdatedAccountNumber(characters, i, '|', faultyAccount.AccountNumber, digitToConvert);
                    if (!string.IsNullOrEmpty(updatedAccount)) returnValue.Add(updatedAccount);
                }
                else
                {
                    updatedAccount = GetUpdatedAccountNumber(characters, i, ' ', faultyAccount.AccountNumber, digitToConvert);
                    if (!string.IsNullOrEmpty(updatedAccount)) returnValue.Add(updatedAccount);
                }
            }
            return returnValue;
        }

        private string GetUpdatedAccountNumber(string digitCharacters, int characterIndex, char replacementValue, string accountNumber, int digitIndex)
        {
            var characters = ReplaceIndexCharacter(digitCharacters, characterIndex, replacementValue);
            var knownDigit = GetDigit(characters);
            if (knownDigit != null)
            {
                var newAccountNumber = ReplaceIndexCharacter(accountNumber, digitIndex, Convert.ToChar(knownDigit.Value));
                if (IsValidAccountNumber(newAccountNumber)) return newAccountNumber;
            }
            return string.Empty;
        }

        private string ReplaceIndexCharacter(string stringToUpdate, int indexToChange, char newValue)
        {
            var stringBuilder = new StringBuilder(stringToUpdate);
            stringBuilder[indexToChange] = newValue;
            return stringBuilder.ToString();
        }

        private Digit GetDigit(string digitCharacters)
        {
            var parser = new DigitParser();
            return parser.GetDigit(digitCharacters);
        }

        private bool IsValidAccountNumber(string accountNumber)
        {
            if (accountNumber.Contains("?")) return false;
            var service = new AccountVerificationService();
            return service.AccountIsValid(accountNumber);
        }
    }
}
