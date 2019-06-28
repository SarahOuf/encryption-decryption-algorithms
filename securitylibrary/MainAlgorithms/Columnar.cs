using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        //function to get permutation from a givin list of a generic type
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> Permutedlist)
        {
            var arr = Permutedlist as T[] ?? Permutedlist.ToArray();
            // get factorial of len
            var factorials = Enumerable.Range(0, arr.Length + 1).Select(Factorial).ToArray();

            for (var i = 0L; i < factorials[arr.Length]; i++)
            {
                // generate sequence and pass it to generate permutations
                var sequence = GenerateSequence(i, arr.Length - 1, factorials);
                yield return GeneratePermutation(arr, sequence);
            }
        }

        private static IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
        {
            var c = (T[])array.Clone();

            for (int i = 0; i < c.Length - 1; i++)
            {
                Swap(ref c[i], ref c[i + sequence[i]]);
            }

            return c;
        }

        private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
        {
            var seq = new int[size];

            for (var j = 0; j < seq.Length; j++)
            {
                var factorial = factorials[seq.Length - j];

                seq[j] = (int)(number / factorial);
                number = (int)(number % factorial);
            }

            return seq;
        }

        static void Swap<T>(ref T first, ref T sec)
        {
            T temp = first;
            first = sec;
            sec = temp;
        }

        private static long Factorial(int num)
        {
            long result = num;

            for (int i = 1; i < num; i++)
            {
                result = result * i;
            }

            return result;
        }

        public List<int> Analyse(string plainText, string cipherText)
        {
            List<int> key = new List<int>();
            bool check = false;
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            string cipherChecker = null;
            for (int i = 2; i <= plainText.Length - 1; i++)
            {
                IEnumerable<IEnumerable<int>> result = GetPermutations(Enumerable.Range(1, i));
                foreach (var value in result)
                {
                    cipherChecker = Encrypt(plainText, value.ToList());
                    cipherChecker = cipherChecker.ToLower();
                    if (cipherChecker == cipherText)
                    {
                        check = true;
                        key = value.ToList();
                        break;
                    }
                }
                if (check == true)
                {
                    break;
                }

            }
            return key;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            int columnNumber = key.Max();
            int rowNum = 0;
            int index = -1;
            char[,] matrix;
            string plainText = null;
            double rowNumber = (double)cipherText.Length / key.Count;
            if (rowNumber % 1 != 0)
            {
                rowNumber = Math.Ceiling(rowNumber);
                rowNum = (int)rowNumber;
                matrix = new char[rowNum, columnNumber];
            }
            else
            {
                rowNum = (int)rowNumber;
                matrix = new char[rowNum, columnNumber];
            }
            int length = (rowNum * columnNumber) - cipherText.Length;
            StringBuilder cipherTextBuilder = new StringBuilder();
            cipherTextBuilder.Append(cipherText);
            for (int i = 0; i < length; i++)
            {
                cipherTextBuilder.Append(" ");
            }
            cipherText = cipherTextBuilder.ToString();
            char[,] matrix2 = new char[rowNum, columnNumber];
            int listIndex = 0;
            for (int column = 0; column < columnNumber; column++)
            {
                for (int rownum = 0; rownum < rowNum; rownum++)
                {
                    index++;
                    matrix[rownum, column] = cipherText[index];
                }

            }
            index = -1;
            for (int column = 0; column < columnNumber; column++)
            {
                index++;
                listIndex = key[index];
                listIndex -= 1;
                for (int rownum = 0; rownum < rowNum; rownum++)
                {
                    matrix2[rownum, column] = matrix[rownum, listIndex];
                }
            }
            for (int rownum = 0; rownum < rowNum; rownum++)
            {
                for (int colnum = 0; colnum < columnNumber; colnum++)
                {
                    plainText += matrix2[rownum, colnum];
                }
            }
            plainText = plainText.Replace(" ", String.Empty);
            return plainText.ToUpper();
        }

        public string Encrypt(string plainText, List<int> key)
        {
            int columnNumber = key.Max();
            int rowNum = 0;
            int index = -1;
            char[,] matrix;
            string cipherText = null;
            double rowNumber = (double)plainText.Length / key.Count;
            if (rowNumber % 1 != 0)
            {
                rowNumber = Math.Ceiling(rowNumber);
                rowNum = (int)rowNumber;
                matrix = new char[rowNum, columnNumber];
            }
            else
            {
                rowNum = (int)rowNumber;
                matrix = new char[rowNum, columnNumber];
            }
            int length = (rowNum * columnNumber) - plainText.Length;
            StringBuilder plainTextBuilder = new StringBuilder();
            plainTextBuilder.Append(plainText);
            for (int i = 0; i < length; i++)
            {
                plainTextBuilder.Append(" ");
            }
            plainText = plainTextBuilder.ToString();
            char[,] matrix2 = new char[rowNum, columnNumber];
            int listIndex = 0;
            for (int rownum = 0; rownum < rowNum; rownum++)
            {
                for (int colnum = 0; colnum < columnNumber; colnum++)
                {
                    index++;
                    matrix[rownum, colnum] = plainText[index];
                }
            }
            index = -1;
            for (int column = 0; column < columnNumber; column++)
            {
                index++;
                listIndex = key[index];
                listIndex -= 1;
                for (int rownum = 0; rownum < rowNum; rownum++)
                {
                    matrix2[rownum, listIndex] = matrix[rownum, column];
                }
            }
            for (int column = 0; column < columnNumber; column++)
            {
                for (int rownum = 0; rownum < rowNum; rownum++)
                {
                    cipherText += matrix2[rownum, column];

                }
            }

            cipherText = cipherText.Replace(" ", String.Empty);
            return cipherText.ToUpper();
        }
    }
}
