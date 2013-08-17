using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOCRKata
{
    class DigitParser
    {
        private const int lineLength = 27;
        private const int doubleLineLength = 54;
        private const int correctTextLength = 81;
        private const int characterWidth = 3;
        private const int accountLength = 9;

        public List<Digit> KnownDigits { get; set; }

        private void LoadKnownDigits()
        {
            KnownDigits = new List<Digit>
            {
                new Digit { Characters = "     |  |", Value = "1"},
                new Digit { Characters = " _  _||_ ", Value = "2"},
                new Digit { Characters = " _  _| _|", Value = "3"},
                new Digit { Characters = "   |_|  |", Value = "4"},
                new Digit { Characters = " _ |_  _|", Value = "5"},
                new Digit { Characters = " _ |_ |_|", Value = "6"},
                new Digit { Characters = " _   |  |", Value = "7"},
                new Digit { Characters = " _ |_||_|", Value = "8"},
                new Digit { Characters = " _ |_| _|", Value = "9"},
                new Digit { Characters = " _ | ||_|", Value = "0"}
            };
        }

        public Account GetAccount(string text)
        {
            var digits = new List<Digit>();
            if (text.Length != correctTextLength) throw new ArgumentException(string.Format("{0} is an invalid number of characters", text.Length));
            for (int i = 0; i < accountLength; i++)
            {
                var currentCharacterTop = text.Substring(i * characterWidth, characterWidth);
                var currentCharacterMiddle = text.Substring(i * characterWidth + lineLength, characterWidth);
                var currentCharacterBottom = text.Substring(i * characterWidth + doubleLineLength, characterWidth);
                var currentCharacter = string.Concat(currentCharacterTop, currentCharacterMiddle, currentCharacterBottom);
                var knownDigit = KnownDigits.FirstOrDefault(digit => digit.Characters.Equals(currentCharacter));
                var digitValue = knownDigit == null ? "?" : knownDigit.Value;
                digits.Add(new Digit { Characters = currentCharacter, Value = digitValue });
            }
            return BuildAccount(digits);
        }

        private Account BuildAccount(List<Digit> digits)
        {
            var statusCode = string.Empty;
            var checksum = -1;
            var accountNumber = string.Join("", digits.Select(digit => digit.Value).ToArray());
            if (accountNumber.Contains("?"))
            {
                statusCode = "ILL";
            }
            else
            {
                var service = new AccountVerificationService();
                checksum = service.GetAccountChecksum(accountNumber);
                if (!checksum.Equals(0)) statusCode = "ERR";
            }
            return new Account
            {
                AccountNumber = accountNumber,
                CheckSum = checksum,
                StatusCode = statusCode,
                Digits = digits
            };
        }

        public Digit GetDigit(string digitCharacters)
        {
            return KnownDigits.FirstOrDefault(digit => digit.Characters.Equals(digitCharacters));
        }

        public DigitParser()
        {
            LoadKnownDigits();
        }
    }
}
