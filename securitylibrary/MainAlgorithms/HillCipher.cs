using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SecurityLibrary
{
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> DotProduct(List<int> Matrix, List<int> Vector)
        {
            int stride = (int)Math.Sqrt(Matrix.Count);
            int index = 0;
            int m = (int)Math.Sqrt(Vector.Count);
            int dotval = 0;

            List<int> cipher = new List<int>();
            for (int i = 0; i < Matrix.Count; i += stride)
            {
                for (int j = i; j < i + stride; j++)
                {
                    dotval += Vector[index] * Matrix[j];
                    index++;
                }
                cipher.Add(dotval % 26);
                dotval = 0;
                index = 0;

            }


            return cipher;
        }
        public int Det(List<int> Matrix)
        {
            int dim = (int)Math.Sqrt(Matrix.Count);
            int det = 0;
            if (dim == 1)
            {
                return Matrix[0];
            }
            if (dim == 2)
            {
                return (Matrix[0] * Matrix[3] - Matrix[1] * Matrix[2]);
            }
            else
            {
                for (int i = 0; i < dim; i++)
                {
                    List<int> ConvMatrix = new List<int>();
                    for (int j = 0; j < (dim) * (dim); j++)
                    {
                        if (!(j < dim) && (j - i) % dim != 0)
                            ConvMatrix.Add(Matrix[j]);
                    }
                    if (i % 2 != 0)
                    {
                        det += (-1 * Matrix[i]) * (Det(ConvMatrix));
                    }
                    else
                    {
                        det += (Matrix[i]) * (Det(ConvMatrix));

                    }


                }
            }
            return det;
        }
        public List<int> Transpose(List<int> matrix)
        {
            int dim = (int)Math.Sqrt(matrix.Count);
            List<int> Matrix_Transposed = new List<int>();
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    Matrix_Transposed.Add(matrix[i + (j * dim)]);
                }
            }
            return Matrix_Transposed;
        }
        public List<int> D2ModInv(List<int> plain, List<int> cipher, int rows2or3)
        {
            List<int> Key = new List<int>();
            int cipherIndx = 0;
            for (int i = 0; i < rows2or3; i++)
            {
                bool flag = false;
                for (int x = 0; x < 26; x++)
                {
                    for (int y = 0; y < 26; y++)
                    {
                        for (int z = 0; z < 26; z++)
                        {
                            int cnt = 0, colr = 0;
                            for (int j = 0; j < plain.Count(); j += rows2or3)
                            {
                                int ans = -1;
                                if (rows2or3 == 2)
                                {
                                    ans = x % 26 * plain[j] % 26 + y % 26 * plain[j + 1] % 26;

                                }
                                else
                                {
                                    ans = x % 26 * plain[j] % 26 + y % 26 * plain[j + 1] % 26 + z % 26 * plain[j + 2];

                                }
                                ans %= 26;
                                cipherIndx = i + j;
                                colr++;
                                if (ans == cipher[cipherIndx])
                                {
                                    cnt++;
                                }
                            }

                            if (cnt == plain.Count() / rows2or3)
                            {
                                if (rows2or3 == 2)
                                {
                                    Key.Add(x);
                                    Key.Add(y);

                                }
                                else
                                {
                                    Key.Add(x);
                                    Key.Add(y);
                                    Key.Add(z);


                                }
                                flag = true;
                                break;
                            }

                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }


            }
            int nn = 0;




            return Key;
        }
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {


            return D2ModInv(plainText, cipherText, 2); ;
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int Det_key = Det(key) % 26;
            while (Det_key < 0)
            {
                Det_key += 26;
            }
            List<int> keyInverse = new List<int>();
            int dim = (int)Math.Sqrt(key.Count);
            int b = 0;
            int index = 1;
            while (b != 1)
            {
                b = (index * Det_key) % 26;
                index++;

            }
            b = (index - 1);


            for (int i = 0; i < (key.Count); i++)
            {
                int D = (int)Math.Sqrt(key.Count);
                int row = i / D;
                int col = i % D;
                int k = b * ((int)Math.Pow(-1, row + col));
                List<int> mat = new List<int>();
                for (int j = 0; j < key.Count; j++)
                {
                    if ((!(j / D == row) && !(j % D == col)))
                    {
                        mat.Add(key[j]);
                    }
                }

                k = (k * Det(mat)) % 26;
                while (k < 0)
                {
                    k += 26;
                }
                keyInverse.Add(k);


            }
            keyInverse = Transpose(keyInverse);
            List<int> test = Encrypt(cipherText, keyInverse);
            return Encrypt(cipherText, keyInverse);
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> cipher = new List<int>();
            int index2 = 0;
            int col_size = (int)Math.Sqrt(key.Count);
            int cols = (int)plainText.Count / (int)Math.Sqrt(key.Count);
            for (int i = 0; i < plainText.Count; i += col_size)
            {
                List<int> col = new List<int>();
                for (int j = i; j < i + col_size; j++)
                {
                    col.Add(plainText[j]);
                }
                cipher.AddRange(DotProduct(key, col));
            }
            return cipher;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            return D2ModInv(plainText, cipherText, 3);
        }

    }
}

