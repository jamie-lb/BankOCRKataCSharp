using System;
using System.Collections.Generic;
using System.IO;

namespace BankOCRKata
{
    class FileReader
    {
        public List<Account> ReadAccountNumbers(string fileName)
        {
            var accounts = new List<Account>();
            var textEntries = GetFileTextEntries(fileName);
            var parser = new DigitParser();
            foreach (var entry in textEntries)
            {
                accounts.Add(parser.GetAccount(entry));
            }
            return accounts;
        }

        private List<String> GetFileTextEntries(string fileName)
        {
            var numberStrings = new List<String>();
            var currentString = string.Empty;
            using (var file = new StreamReader(fileName))
            {
                while (!file.EndOfStream)
                {
                    currentString += file.ReadLine();
                    currentString += file.ReadLine();
                    currentString += file.ReadLine();
                    numberStrings.Add(currentString);
                    currentString = string.Empty;
                    file.ReadLine();
                }
            }
            return numberStrings;
        }
    }
}
