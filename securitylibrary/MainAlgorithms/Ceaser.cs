using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            //throw new NotImplementedException();
            String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string mainPlain = plainText.ToUpper();

            char[] arr = new char[mainPlain.Length];

            for (int i = 0; i < mainPlain.Length; i++)
            {
                int a = alphabet.IndexOf(mainPlain[i]);
                int c = (a + key) % 26;
                arr[i] = alphabet.ElementAt(c);

            }
            var builder = new StringBuilder();
            Array.ForEach(arr, x => builder.Append(x));
            string result = builder.ToString();

            return result;
        }

        public string Decrypt(string cipherText, int key)
        {
            //throw new NotImplementedException();
            String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string mainPlain = cipherText.ToUpper();

            char[] arr = new char[mainPlain.Length];

            for (int i = 0; i < mainPlain.Length; i++)
            {
                int a = alphabet.IndexOf(mainPlain[i]);
                int c = (a - key) % 26;
                if (c < 0)
                {
                    c = c + alphabet.Length;
                }
                arr[i] = alphabet.ElementAt(c);

            }
            var builder = new StringBuilder();
            Array.ForEach(arr, x => builder.Append(x));
            string result = builder.ToString();

            return result;
        }

        public int Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string plaintext = plainText.ToUpper();
            string ciphertext = cipherText.ToUpper();
            int key = 0;
            for (int i = 0; i < 26; i++)
            {
                int a = alphabet.IndexOf(plaintext[0]);
                int c = (a + i) % 26;
                char ch = alphabet.ElementAt(c);
                if (ch == ciphertext[0])
                {
                    key = i;
                    break;
                }
            }

            return key;
        }
    }
}
