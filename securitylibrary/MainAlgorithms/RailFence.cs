using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            //char letter = cipherText[1];
            //int index = 0;
            int key = 0;
            string cipherChecker = null;
            /*for (int i = 0; i < plainText.Length;i++)
            {
                if (plainText[i] == letter && plainText[i + 1] == letter || plainText[i] == letter && plainText[i - 1] == letter)
                {
                    index = i+1;
                    break;
                }
                else if(plainText[i] == letter)
                {
                    index = i;
                    break;
                }
            }
            key = index;*/
            for (int i = 2; i <= plainText.Length - 1; i++)
            {
                cipherChecker = Encrypt(plainText, i);
                cipherChecker = cipherChecker.ToLower();
                if (cipherChecker == cipherText)
                {
                    key = i;
                    break;
                }

            }
            return key;
        }

        public string Decrypt(string cipherText, int key)
        {
            int index = -1;
            int colnum = 0;
            string plainText = null;
            char[,] matrix;
            double col = (double)cipherText.Length / key;
            string num = col.ToString();
            int indxx = 0;
            if (col % 1 != 0)
            {

                float numm = float.Parse(num);
                col = Math.Ceiling(col);
                colnum = (int)col;

                matrix = new char[key, colnum];

            }
            else
            {

                float numm = float.Parse(num);
                col = Math.Ceiling(col);
                colnum = (int)col;
                matrix = new char[key, colnum];
            }
            int length = (key * colnum) - cipherText.Length;
            StringBuilder cipherTextBuilder = new StringBuilder();
            cipherTextBuilder.Append(cipherText);
            for (int i = 0; i < length; i++)
            {
                cipherTextBuilder.Append(" ");
            }
            cipherText = cipherTextBuilder.ToString();
            for (int row = 0; row < key; row++)
            {
                for (int column = 0; column < colnum; column++)
                {
                    index++;
                    matrix[row, column] = (char)cipherText[index];

                }
            }
            for (int column = 0; column < colnum; column++)
            {
                for (int row = 0; row < key; row++)
                {
                    plainText += matrix[row, column];
                }
            }
            plainText = plainText.Replace(" ", String.Empty);
            return plainText.ToUpper();
        }




        public string Encrypt(string plainText, int key)
        {
            int index = -1;
            int colnum = 0;
            string cipherText = null;
            char[,] matrix;
            double col = (double)plainText.Length / key;
            string num = col.ToString();
            int indxx = 0;
            if (col % 1 != 0)
            {

                float numm = float.Parse(num);
                col = Math.Ceiling(col);
                colnum = (int)col;

                matrix = new char[key, colnum];

            }
            else
            {


                float numm = float.Parse(num);
                col = Math.Ceiling(col);
                colnum = (int)col;
                matrix = new char[key, colnum];
            }
            int length = (key * colnum) - plainText.Length;
            StringBuilder plainTextBuilder = new StringBuilder();
            plainTextBuilder.Append(plainText);
            for (int i = 0; i < length; i++)
            {
                plainTextBuilder.Append(" ");
            }
            plainText = plainTextBuilder.ToString();
            for (int column = 0; column < colnum; column++)
            {
                for (int row = 0; row < key; row++)
                {
                    index++;
                    matrix[row, column] = (char)plainText[index];
                }
            }

            for (int row = 0; row < key; row++)
            {
                for (int column = 0; column < colnum; column++)
                {
                    cipherText += matrix[row, column];
                }
            }
            cipherText = cipherText.Replace(" ", String.Empty);
            return cipherText.ToUpper();
        }
    }
}
