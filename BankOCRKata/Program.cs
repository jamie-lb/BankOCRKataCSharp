using System;

namespace BankOCRKata
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new FileReader();
            var service = new AccountResolvingService();
            var accounts = reader.ReadAccountNumbers("BankOCRText.txt");
            foreach (var account in accounts)
            {
                if (!string.IsNullOrEmpty(account.StatusCode))
                {
                    var correctedAccount = service.RepairedAccount(account);
                    Console.WriteLine("{0} {1}", correctedAccount.AccountNumber, correctedAccount.StatusCode);
                }
                else
                {
                    Console.WriteLine("{0} {1}", account.AccountNumber, account.StatusCode);
                }
            }
            Console.ReadLine();
        }
    }
}
