using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            string key = "";
            int chind;
            for (int i = 0; i < plainText.Length; i++)
            {
                chind = (alpha.IndexOf(cipherText[i]) - alpha.IndexOf(plainText[i]));
                if (chind < 0)
                {
                    while (chind < 0)
                    {
                        chind += 26;
                    }
                }
                else
                    chind = chind % 26;

                key += alpha[chind];
            }
            int dx = 0;
            while (true)
            {
                dx = getind(dx + 1, plainText[0], key);
               if(plainText.Contains(key.Substring(dx)))
                {
                    break;
                }
            
            }
            return key.Substring(0, dx);
        }
        public int getind(int startind, char found, string st)
        {
            for (int i = startind; i < st.Length; i++)
            {
                if (st[i] == found)
                {
                    return i;
                }

            }
            return -1;
        }
        public string Decrypt(string cipherText, string key)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            int deff = cipherText.Length - key.Length;
            string k = key;
            string plaintext = "";
            int chind;
            for (int i = 0; i < key.Length; i++)
            {
                chind = (alpha.IndexOf(cipherText[i]) - alpha.IndexOf(key[i]));
                if (chind < 0)
                {
                    while (chind < 0)
                    {
                        chind += 26;
                    }
                }
                else
                    chind = chind % 26;

                plaintext += alpha[chind];
                if(deff>0)
                {
                    key += alpha[chind];
                    deff--;
                }
            }

            return plaintext;
        }

        public string Encrypt(string plainText, string key)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            plainText = plainText.ToLower();
            key = key.ToLower();
            int deff = plainText.Length - key.Length;
            if (deff > 0)
            {
              
                key += plainText.Substring(0, deff);
            }
            string ciphertext = "";
            int chind;
            for (int i = 0; i < key.Length; i++)
            {
                chind = (alpha.IndexOf(plainText[i]) + alpha.IndexOf(key[i])) % 26;
                ciphertext += alpha[chind];
            }
            return ciphertext;
        }
    }
}
