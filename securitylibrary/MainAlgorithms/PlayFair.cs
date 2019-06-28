using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string GetCorrectInput(string plaintext)
        {
            /////////prepare the input
            string correctInput = "";
            int i;
            for (i = 0; i < plaintext.Length - 1; i += 2)
            {
                char ch1 = plaintext[i];
                char ch2 = plaintext[i + 1];

                if (ch1 == 'J')
                {
                    ch1 = 'I';
                }
                if (ch2 == 'J')
                {
                    ch2 = 'I';
                }

                if (ch1 == ch2)
                {
                    correctInput += ch1;
                    correctInput += 'X';
                    //correctInput += ch2;
                    i--;
                }
                else
                {
                    correctInput += ch1;
                    correctInput += ch2;
                }
            }

            if (plaintext.Length != i)
            {
                correctInput += plaintext[plaintext.Length - 1];
            }

            if (correctInput.Length % 2 != 0)
            {
                correctInput += 'X';
            }
            return correctInput;
        }

        public List<char> GetTable(string cipherKey)
        {
            ////////////////////generate 5x5 table
            List<char> table = new List<char>();
            string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";

            foreach (char ch in cipherKey)
            {
                if (table.Contains(ch) == false)
                {
                    table.Add(ch);
                }
            }

            foreach (char ch in alphabet)
            {
                if (table.Contains(ch) == false)
                {
                    table.Add(ch);
                }
            }
            return table;
        }

        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();            
            string Key = key.ToUpper();
            string ciphertext = cipherText.ToUpper();
            string Plaintext = "";
            
            List<char> table = GetTable(Key);

            for (int i = 0; i < ciphertext.Length - 1; i += 2)
            {
                char ch1 = ciphertext[i];
                char ch2 = ciphertext[i + 1];

                if (ch1 == 'J')
                {
                    ch1 = 'I';
                }
                if (ch2 == 'J')
                {
                    ch2 = 'I';
                }

                int row1 = table.IndexOf(ch1) / 5;
                int col1 = table.IndexOf(ch1) % 5;

                int row2 = table.IndexOf(ch2) / 5;
                int col2 = table.IndexOf(ch2) % 5;

                if (row1 == row2)
                {
                    int a = col1 - 1;
                    if (a < 0)
                    {
                        a = a + 5;
                    }
                    int c = col2 - 1;
                    if (c < 0)
                    {
                        c = c + 5;
                    }

                    Plaintext += table[row1 * 5 + a];
                    Plaintext += table[row2 * 5 + c];
                }
                else if (col1 == col2)
                {
                    int a = row1 - 1;
                    if (a < 0)
                    {
                        a = a + 5;
                    }
                    int c = row2 - 1;
                    if (c < 0)
                    {
                        c = c + 5;
                    }

                    Plaintext += table[(a) * 5 + col1];
                    Plaintext += table[(c) * 5 + col2];
                }
                else
                {
                    Plaintext += table[row1 * 5 + col2];
                    Plaintext += table[row2 * 5 + col1];
                }


            }
            
            string correctoutput = "";

            for (int i = 0; i < Plaintext.Length - 1; i++)
            {
                char ch = Plaintext[i];

                if (ch == 'X')
                {
                    if (i % 2 != 0)
                    {
                        //Console.WriteLine(i);
                        char ch1 = Plaintext[i - 1];
                        char ch2 = Plaintext[i + 1];
                        if (ch1 == ch2)
                        {
                            //correctoutput += ch1;
                            correctoutput += ch2;
                            i++;
                        }
                        else
                        {
                            correctoutput += ch;
                        }

                    }
                    else
                    {
                        correctoutput += ch;
                    }
                }
                else
                {
                    correctoutput += ch;
                }
            }
            if (Plaintext[Plaintext.Length - 1] != 'X')
            {
                correctoutput += Plaintext[Plaintext.Length - 1];
            }
            return correctoutput;
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            string Plaintext = plainText.ToUpper();
            string Key = key.ToUpper();            
            string cipherText = "";

            string correctInput = GetCorrectInput(Plaintext);
            List<char> table = GetTable(Key);

            for (int i = 0; i < correctInput.Length - 1; i += 2)
            {
                char ch1 = correctInput[i];
                char ch2 = correctInput[i + 1];

                int row1 = table.IndexOf(ch1) / 5;
                int col1 = table.IndexOf(ch1) % 5;

                int row2 = table.IndexOf(ch2) / 5;
                int col2 = table.IndexOf(ch2) % 5;

                if (row1 == row2)
                {
                    cipherText += table[row1 * 5 + (col1 + 1) % 5];
                    cipherText += table[row2 * 5 + (col2 + 1) % 5];
                }
                else if (col1 == col2)
                {
                    cipherText += table[((row1 + 1) % 5) * 5 + col1];
                    cipherText += table[((row2 + 1) % 5) * 5 + col2];
                }
                else
                {
                    cipherText += table[row1 * 5 + col2];
                    cipherText += table[row2 * 5 + col1];
                }
            }

            return cipherText;

        }
    }
}
