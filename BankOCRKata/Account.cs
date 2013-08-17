using System.Collections.Generic;
namespace BankOCRKata
{
    class Account
    {
        public string AccountNumber { get; set; }
        public List<Digit> Digits { get; set; }
        public int CheckSum { get; set; }
        public string StatusCode { get; set; }
    }
}
