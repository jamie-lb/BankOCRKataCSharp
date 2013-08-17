using System;

namespace BankOCRKata
{
    class AccountVerificationService
    {
        public bool AccountIsValid(string accountNumber)
        {
            var checksum = GetAccountChecksum(accountNumber);
            return checksum.Equals(0);
        }

        public int GetAccountChecksum(string accountNumber)
        {
            var totalSum = 0;
            for (int i = 0; i < accountNumber.Length; i++)
            {
                var currentDigit = Convert.ToInt32(accountNumber.Substring(i, 1));
                var operand = 9 - i;
                totalSum += currentDigit * operand;
            }
            return totalSum % 11;
        }
    }
}
