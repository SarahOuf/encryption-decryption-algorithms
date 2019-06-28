using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            string Plaintext = plainText.ToLower();
            string Key = "";
            String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower();
            string ciphertext = cipherText.ToLower();
            char[] arr = new char[] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };

            for (int i = 0; i < Plaintext.Length; i++)
            {
                char Plainchar = Plaintext[i];
                char Cipherchar = ciphertext[i];
                int alphaIndex = alphabet.IndexOf(Plainchar);
                arr[alphaIndex] = Cipherchar;
            }
            char nextChar;
            int index = 0;
            for (int i = 0; i < arr.Length; i++)
            {

                char ch = arr[i];
                if (ch == ' ')
                {
                    nextChar = ch;
                    index = i;
                    break;
                }
            }
            int alphaIndex2 = 0;
            for (int i = index; i < arr.Length; i++)
            {

                char ch = arr[i];
                if (ch == ' ')
                {
                    char ch2 = arr[i - 1];                    
                    alphaIndex2 = alphabet.IndexOf(ch2);
                    alphaIndex2 = (alphaIndex2 + 1) % 26;
                    while (arr.Contains(alphabet[alphaIndex2]) == true)
                    {
                        alphaIndex2 = (alphaIndex2 + 1) % 26;
                    }
                    if (arr.Contains(alphabet[alphaIndex2]) == false)
                    {

                        arr[i] = alphabet[alphaIndex2];
                    }

                }
            }

            var builder = new StringBuilder();
            Array.ForEach(arr, x => builder.Append(x));
            string result = builder.ToString();
            //Console.WriteLine(result);

            return result;

        }

        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();          
            String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string mainKey = key.ToUpper();
            string cipher = cipherText.ToUpper();
            char[] arr = new char[cipher.Length];

            for (int i = 0; i < cipher.Length; i++)
            {
                int a = mainKey.IndexOf(cipher[i]);
                arr[i] = alphabet.ElementAt(a);

            }
            var builder = new StringBuilder();
            Array.ForEach(arr, x => builder.Append(x));
            string result = builder.ToString();

            return result;

        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            string mainPlain = plainText.ToUpper();
            String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            char[] arr = new char[mainPlain.Length];

            for (int i = 0; i < mainPlain.Length; i++)
            {
                int a = alphabet.IndexOf(mainPlain[i]);
                arr[i] = key.ElementAt(a);

            }
            var builder = new StringBuilder();
            Array.ForEach(arr, x => builder.Append(x));
            string result = builder.ToString();

            return result;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            //throw new NotImplementedException();
            string cipherText = cipher.ToLower();
            string frequency = "ETAOINSRHLDCUMFPGWYBVKXJQZ".ToLower();

            var charLookup = cipherText.GroupBy(c => c).Select(c => new { Char = c.Key, Count = c.Count() });

            var charsInAscOrder = charLookup.OrderByDescending(c => c.Count);
            string key = "";
            foreach (var c in charsInAscOrder)
            {
                key += c.Char;
            }

            char[] arr1 = new char[cipherText.Length];

            for (int i = 0; i < cipherText.Length; i++)
            {                
                int a = key.IndexOf(cipherText[i]);              
                arr1[i] = frequency.ElementAt(a);
            }

            var builder = new StringBuilder();
            Array.ForEach(arr1, x => builder.Append(x));
            string result = builder.ToString();
            return result;

        }
    }
}
