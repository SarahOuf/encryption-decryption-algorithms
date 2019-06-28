using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
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
            string k = key;
            int dx=0;
            bool flag = false;
          while(true)
            {
                dx = getind(dx+1,k[0],k);
                flag = false;
                for (int l=dx, j = 0; j < dx; j++,l++)
                {
                    
                    if(l<k.Length)
                    {
                        if(k[l]!=k[j])
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if(!flag)
                {
                    break;
                }
            }
            return k.Substring(0, dx);
        }
        public int getind(int startind,char found,string st)
        {
            for (int i = startind; i < st.Length; i++)
            {
                if(st[i]==found)
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
            if (deff > 0)
            {
                while (deff >= key.Length)
                {
                    deff -= key.Length;
                    key += k;
                }
                key += key.Substring(0, deff);
            }
            string plaintext = "";
            int chind;
            for (int i = 0; i < key.Length; i++)
            {
                chind = (alpha.IndexOf(cipherText[i]) - alpha.IndexOf(key[i])) ;
                if(chind<0)
                {
                    while(chind<0)
                    {
                        chind += 26;
                    }
                }
                else
                chind = chind % 26;

                plaintext += alpha[chind];
            }

            return plaintext;
        }

        public string Encrypt(string plainText, string key)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            plainText = plainText.ToLower();
            key = key.ToLower();
            int deff = plainText.Length - key.Length;
            string k = key;
            if (deff > 0)
            {
                while(deff>=key.Length)
                {
                    deff -= key.Length;
                    key += k;
                }
                key+= key.Substring(0,deff);
            }
            string ciphertext = "";
            int chind;
            for (int i = 0; i < key.Length; i++)
            {
                chind = (alpha.IndexOf(plainText[i]) +alpha.IndexOf( key[i]))%26;
                ciphertext += alpha[chind];
            }
            return ciphertext;
        }
    }
}