using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        public void Rotate(ref string[] keyVector)
        {
            string x = keyVector[0];
            for (int i = 0; i < keyVector.Length - 1; i++)
            {
                keyVector[i] = keyVector[i + 1];
            }
            keyVector[keyVector.Length - 1] = x;
        }

        public string XOR(string string1, string string2)
        {
            int intString1 = int.Parse(string1, System.Globalization.NumberStyles.HexNumber);
            int intString2 = int.Parse(string2, System.Globalization.NumberStyles.HexNumber);
            int xored = intString1 ^ intString2;
            string result = xored.ToString("X").PadLeft(2, '0');

            return result;
        }
        public void XorRc(ref string[] keyVector, int rcon)
        {
            string sRcon = rcon.ToString("X");
            string[] rconVector = new string[4] { rcon.ToString("X"), 0.ToString("X"), 0.ToString("X"), 0.ToString("X") };

            for (int i = 0; i < keyVector.Length; i++)
            {
                keyVector[i] = XOR(keyVector[i], rconVector[i]);
            }
            //rcon = (rcon << 1) ^ (0x11b & -(rcon >> 7));
        }

        public string[] sboxKey(string[] cvector)
        {
            string[] arr = new string[4];
            string[,] S_box = new string[16, 16];
            String input = File.ReadAllText("sbox.txt");
            int q = 0, r = 0;
            foreach (var row in input.Split('\n'))
            {
                r = 0;
                foreach (var col in row.Trim().Split('\t'))
                {
                    S_box[q, r] = col.Trim();
                    r++;
                }
                q++;
            }
            int x, y;

            for (int j = 0; j < 4; j++)
            {
                string current_pos = cvector[j];

                x = current_pos[0];
                y = current_pos[1];
                if (x >= 'A' && x <= 'Z')
                {
                    x -= 'A';
                    x += 10;
                }
                else if (x >= 'a' && x <= 'z')
                {
                    x -= 'a';
                    x += 10;
                }
                else
                {
                    x -= '0';
                }

                if (y >= 'A' && y <= 'Z')
                {
                    y -= 'A';
                    y += 10;
                }
                else if (y >= 'a' && y <= 'z')
                {
                    y -= 'a';
                    y += 10;
                }
                else
                {
                    y -= '0';
                }
                arr[j] = S_box[x, y];
            }

            return arr;
        }
        public string[,] stringToMatrix(string s)
        {
            string[,] hexMatrix = new string[4, 4];
            int w = 2;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    hexMatrix[j, i] = Convert.ToString(s[w]) + Convert.ToString(s[w + 1]);
                    w += 2;
                }
            }
            return hexMatrix;
        }
        public List<string[,]> keyExpansion(string mainKey)
        {
            string[,] hexMatrix = stringToMatrix(mainKey);
            List<string[,]> RoundMatrix = new List<string[,]>(11);
            RoundMatrix.Add(hexMatrix);
            string[] keyVector1 = new string[4];

            int rcon = 1;

            for (int i = 1; i <= 10; i++)
            {
                string[,] tempMatrix = new string[4, 4];
                for (int j = 0; j < 4; j++)
                {
                    keyVector1[j] = hexMatrix[j, 3];
                }
                Rotate(ref keyVector1);
                keyVector1 = sboxKey(keyVector1);
                XorRc(ref keyVector1, rcon);
                for (int x = 0; x < 4; x++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (x == 0)
                        {
                            tempMatrix[j, x] = XOR(hexMatrix[j, x], keyVector1[j]);
                        }
                        else
                        {
                            tempMatrix[j, x] = XOR(hexMatrix[j, x], tempMatrix[j, x - 1]);
                        }

                    }
                }
                RoundMatrix.Add(tempMatrix);
                hexMatrix = tempMatrix;
                rcon = (rcon << 1) ^ (0x11b & -(rcon >> 7));

            }

            return RoundMatrix;
        }
        public string[,] addRoundKey(string[,] plainMatrix, string[,] keyMatrix)
        {
            string[,] result = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[j, i] = XOR(plainMatrix[j, i], keyMatrix[j, i]);
                }
            }
            return result;
        }
        public string[,] sbox(string[,] cmatrix)
        {
            string[,] arr = new string[4, 4];
            string[,] S_box = new string[16, 16];
            String input = File.ReadAllText("sbox.txt");
            int q = 0, r = 0;
            foreach (var row in input.Split('\n'))
            {
                r = 0;
                foreach (var col in row.Trim().Split('\t'))
                {
                    S_box[q, r] = col.Trim();
                    r++;
                }
                q++;
            }
            int x, y;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string current_pos = cmatrix[i, j];

                    x = current_pos[0];
                    y = current_pos[1];
                    if (x >= 'A' && x <= 'Z')
                    {
                        x -= 'A';
                        x += 10;
                    }
                    else if (x >= 'a' && x <= 'z')
                    {
                        x -= 'a';
                        x += 10;
                    }
                    else
                    {
                        x -= '0';
                    }

                    if (y >= 'A' && y <= 'Z')
                    {
                        y -= 'A';
                        y += 10;
                    }
                    else if (y >= 'a' && y <= 'z')
                    {
                        y -= 'a';
                        y += 10;
                    }
                    else
                    {
                        y -= '0';
                    }
                    arr[i, j] = S_box[x, y];
                }
            }


            return arr;
        }
        public string xorshift(string cmatrixji, bool b)
        {
            string retstring = "";
            int xorb1int = Convert.ToInt32("00011011", 2);
            string binaryval = "";
            binaryval = Convert.ToString(Convert.ToInt32(cmatrixji, 16), 2);

            int bin = Convert.ToInt32(binaryval, 2);
            if (binaryval[0] == '1' && binaryval.Length == 8)
            {
                bin = bin << 1;
                binaryval = Convert.ToString(bin, 2);
                if (binaryval.Length > 8)
                {
                    binaryval = binaryval.Substring(1);
                }
                bin = Convert.ToInt32(binaryval, 2);
                retstring = Convert.ToString((xorb1int ^ bin), 2);

            }
            else
            {
                bin = bin << 1;
                binaryval = Convert.ToString(bin, 2);
                if (binaryval.Length > 8)
                {
                    binaryval = binaryval.Substring(1);
                }
                bin = Convert.ToInt32(binaryval, 2);
                retstring = binaryval;
            }
            if (b)
            {
                string hexast = Convert.ToString(Convert.ToInt32(retstring, 2), 16);
                retstring = XOR(hexast, cmatrixji);
                retstring = Convert.ToString(Convert.ToInt32(retstring, 16), 2);
            }
            return retstring;
        }
        public string[,] mixcloumns(string[,] cmatrix)
        {
            string[,] arr = new string[4, 4];
            string[,] constantMat = new string[4, 4] { { "02", "03", "01", "01" }, { "01", "02", "03", "01" }, { "01", "01", "02", "03" }, { "03", "01", "01", "02" } };

            string[] tempres = new string[4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {

                        if (constantMat[i, k].Equals("01"))
                        {
                            tempres[k] = Convert.ToString(Convert.ToInt32(cmatrix[k, j], 16), 2);
                        }
                        else if (constantMat[i, k].Equals("02"))
                        {
                            tempres[k] = xorshift(cmatrix[k, j], false);
                        }
                        else
                        {
                            tempres[k] = xorshift(cmatrix[k, j], true);
                        }
                        tempres[k] = Convert.ToInt32(tempres[k], 2).ToString();
                    }
                    for (int k = 1; k < 4; k++)
                    {
                        tempres[k] = (int.Parse(tempres[k]) ^ int.Parse(tempres[k - 1])).ToString();


                    }
                    arr[i, j] = Convert.ToString(Convert.ToInt32(tempres[3], 10), 16).PadLeft(2, '0');
                }
            }

            return arr;

        }

        public string[,] shiftrows(string[,] cmatrix)
        {
            string[,] arr = new string[4, 4];
            string[] temp = new string[4];
            int ct = 0, h = 0;
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    temp[j] = cmatrix[i, j];
                }
                for (int j = 0; j < 4; j++)
                {
                    int indx = (j - i);
                    indx = (((indx % 4) + 4) % 4);
                    cmatrix[i, indx] = temp[j];
                }
            }

            return cmatrix;
        }
        public string matrixToString(string[,] hexMatrix)
        {
            string result = "0x";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string s = hexMatrix[j, i];
                    result += s;
                }
            }
            return result;
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="key"></param>
        /// <returns></returns>

        public string[,] inverssbox(string[,] cmatrix)
        {
            string[,] arr = new string[4, 4];
            string[,] S_box = new string[16, 16];
            String input = File.ReadAllText("inverseSbox.txt");
            int q = 0, r = 0;
            foreach (var row in input.Split('\n'))
            {
                r = 0;
                foreach (var col in row.Trim().Split('\t'))
                {
                    S_box[q, r] = col.Trim();
                    r++;
                }
                q++;
            }
            int x, y;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string current_pos = cmatrix[i, j];

                    x = current_pos[0];
                    y = current_pos[1];
                    if (x >= 'A' && x <= 'Z')
                    {
                        x -= 'A';
                        x += 10;
                    }
                    else if (x >= 'a' && x <= 'z')
                    {
                        x -= 'a';
                        x += 10;
                    }
                    else
                    {
                        x -= '0';
                    }

                    if (y >= 'A' && y <= 'Z')
                    {
                        y -= 'A';
                        y += 10;
                    }
                    else if (y >= 'a' && y <= 'z')
                    {
                        y -= 'a';
                        y += 10;
                    }
                    else
                    {
                        y -= '0';
                    }
                    arr[i, j] = S_box[x, y];
                }
            }


            return arr;
        }
        public string[,] MixCols(string[,] cmatrix)
        {
            string[,] arr = new string[4, 4];
            string[,] constantMat = new string[4, 4] { { "0E", "0B", "0D", "09" }, { "09", "0E", "0B", "0D" }, { "0D", "09", "0E", "0B" }, { "0B", "0D", "09", "0E" } };
            string[,] E = new string[16, 16]
            {{"01", "03", "05", "0F", "11", "33", "55", "FF", "1A", "2E", "72", "96", "A1", "F8", "13", "35"},
            {"5F", "E1", "38", "48", "D8", "73", "95", "A4", "F7", "02", "06", "0A", "1E", "22", "66", "AA"},
            {"E5", "34", "5C", "E4", "37", "59", "EB", "26", "6A", "BE", "D9", "70", "90", "AB", "E6", "31"},
            {"53", "F5", "04", "0C", "14", "3C", "44", "CC", "4F", "D1", "68", "B8", "D3", "6E", "B2", "CD"},
            {"4C", "D4", "67", "A9", "E0", "3B", "4D", "D7", "62", "A6", "F1", "08", "18", "28", "78", "88"},
            {"83", "9E", "B9", "D0", "6B", "BD", "DC", "7F", "81", "98", "B3", "CE", "49", "DB", "76", "9A"},
            {"B5", "C4", "57", "F9", "10", "30", "50", "F0", "0B", "1D", "27", "69", "BB", "D6", "61", "A3"},
            {"FE", "19", "2B", "7D", "87", "92", "AD", "EC", "2F", "71", "93", "AE", "E9", "20", "60", "A0"},
            {"FB", "16", "3A", "4E", "D2", "6D", "B7", "C2", "5D", "E7", "32", "56", "FA", "15", "3F", "41"},
            {"C3", "5E", "E2", "3D", "47", "C9", "40", "C0", "5B", "ED", "2C", "74", "9C", "BF", "DA", "75"},
            {"9F", "BA", "D5", "64", "AC", "EF", "2A", "7E", "82", "9D", "BC", "DF", "7A", "8E", "89", "80"},
            {"9B", "B6", "C1", "58", "E8", "23", "65", "AF", "EA", "25", "6F", "B1", "C8", "43", "C5", "54"},
            {"FC", "1F", "21", "63", "A5", "F4", "07", "09", "1B", "2D", "77", "99", "B0", "CB", "46", "CA"},
            {"45", "CF", "4A", "DE", "79", "8B", "86", "91", "A8", "E3", "3E", "42", "C6", "51", "F3", "0E"},
            {"12", "36", "5A", "EE", "29", "7B", "8D", "8C", "8F", "8A", "85", "94", "A7", "F2", "0D", "17"},
            {"39", "4B", "DD", "7C", "84", "97", "A2", "FD", "1C", "24", "6C", "B4", "C7", "52", "F6", "01"}};

            string[,] L = new string[16, 16]
           {{ "00", "00", "19", "01", "32", "02", "1A", "C6", "4B", "C7", "1B", "68", "33", "EE", "DF", "03"},
            {"64", "04", "E0", "0E", "34", "8D", "81", "EF", "4C", "71", "08", "C8", "F8", "69", "1C", "C1"},
            {"7D", "C2", "1D", "B5", "F9", "B9", "27", "6A", "4D", "E4", "A6", "72", "9A", "C9", "09", "78"},
            {"65", "2F", "8A", "05", "21", "0F", "E1", "24", "12", "F0", "82", "45", "35", "93", "DA", "8E"},
            {"96", "8F", "DB", "BD", "36", "D0", "CE", "94", "13", "5C", "D2", "F1", "40", "46" ,"83", "38"},
            {"66", "DD", "FD", "30", "BF", "06", "8B", "62", "B3", "25", "E2", "98", "22", "88", "91", "10"},
            {"7E", "6E", "48", "C3", "A3", "B6", "1E", "42", "3A", "6B", "28", "54", "FA", "85", "3D", "BA"},
            {"2B", "79", "0A", "15", "9B", "9F", "5E", "CA", "4E", "D4", "AC", "E5", "F3", "73", "A7", "57"},
            {"AF", "58", "A8", "50", "F4", "EA", "D6", "74", "4F", "AE", "E9", "D5", "E7", "E6", "AD", "E8"},
            {"2C", "D7", "75", "7A", "EB", "16", "0B", "F5", "59", "CB", "5F", "B0", "9C", "A9", "51", "A0"},
            {"7F", "0C", "F6", "6F", "17", "C4", "49", "EC", "D8", "43", "1F", "2D", "A4", "76", "7B", "B7"},
            {"CC", "BB", "3E", "5A", "FB", "60", "B1", "86", "3B", "52", "A1", "6C", "AA", "55", "29", "9D"},
            {"97", "B2", "87", "90", "61", "BE", "DC", "FC", "BC", "95", "CF", "CD", "37", "3F", "5B", "D1"},
            {"53", "39", "84", "3C", "41", "A2", "6D", "47", "14", "2A", "9E", "5D", "56", "F2", "D3" ,"AB"},
            {"44", "11", "92", "D9", "23", "20", "2E", "89", "B4", "7C", "B8", "26", "77", "99", "E3", "A5"},
            {"67", "4A", "ED", "DE", "C5", "31", "FE", "18", "0D", "63", "8C", "80", "C0", "F7", "70", "07"}};
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string xoringHex = "00";
                    for (int k = 0; k < 4; k++)
                    {
                        string ConstString, VarString;
                        ConstString = constantMat[i, k];
                        VarString = cmatrix[k, j];
                        string current_pos = ConstString;

                        Console.WriteLine(ConstString + "--------------" + VarString);
                        int x = current_pos[0];
                        int y = current_pos[1];
                        if (x >= 'A' && x <= 'Z')
                        {
                            x -= 'A';
                            x += 10;
                        }
                        else if (x >= 'a' && x <= 'z')
                        {
                            x -= 'a';
                            x += 10;
                        }
                        else
                        {
                            x -= '0';
                        }

                        if (y >= 'A' && y <= 'Z')
                        {
                            y -= 'A';
                            y += 10;
                        }
                        else if (y >= 'a' && y <= 'z')
                        {
                            y -= 'a';
                            y += 10;
                        }
                        else
                        {
                            y -= '0';
                        }
                        ConstString = L[x, y];

                        current_pos = VarString;

                        x = current_pos[0];
                        y = current_pos[1];
                        if (x >= 'A' && x <= 'Z')
                        {
                            x -= 'A';
                            x += 10;
                        }
                        else if (x >= 'a' && x <= 'z')
                        {
                            x -= 'a';
                            x += 10;
                        }
                        else
                        {
                            x -= '0';
                        }

                        if (y >= 'A' && y <= 'Z')
                        {
                            y -= 'A';
                            y += 10;
                        }
                        else if (y >= 'a' && y <= 'z')
                        {
                            y -= 'a';
                            y += 10;
                        }
                        else
                        {
                            y -= '0';
                        }
                        VarString = L[x, y];
                        int temp = Convert.ToInt32(ConstString, 16);
                        int temp1 = Convert.ToInt32(VarString, 16);
                        int res = temp + temp1;
                        res %= 255;

                        current_pos = Convert.ToString(res, 16).PadLeft(2, '0');
                        x = current_pos[0];
                        y = current_pos[1];
                        if (x >= 'A' && x <= 'Z')
                        {
                            x -= 'A';
                            x += 10;
                        }
                        else if (x >= 'a' && x <= 'z')
                        {
                            x -= 'a';
                            x += 10;
                        }
                        else
                        {
                            x -= '0';
                        }

                        if (y >= 'A' && y <= 'Z')
                        {
                            y -= 'A';
                            y += 10;
                        }
                        else if (y >= 'a' && y <= 'z')
                        {
                            y -= 'a';
                            y += 10;
                        }
                        else
                        {
                            y -= '0';
                        }
                        xoringHex = XOR(xoringHex, E[x, y]);
                    }
                    arr[i, j] = xoringHex;

                }
            }
            return arr;
        }

        public string[,] invShiftrows(string[,] cmatrix)
        {
            string[,] arr = new string[4, 4];
            string[] temp = new string[4];
            int ct = 0, h = 0;
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    temp[j] = cmatrix[i, j];
                }
                for (int j = 0; j < 4; j++)
                {
                    int indx = (j - i);
                    indx = (((indx % 4) + 4) % 4);
                    cmatrix[i, j] = temp[indx];
                }
            }

            return cmatrix;
        }

        public string invXorshift(string cmatrixji, bool b, string constantMatrix)
        {
            string retstring = "";
            int xorb1int = Convert.ToInt32("00011011", 2);
            string binaryval = "";
            binaryval = Convert.ToString(Convert.ToInt32(cmatrixji, 16), 2);
            List<string> list = new List<string>();

            int bin;
            for (int i = 0; i < 3; i++)
            {
                bin = Convert.ToInt32(binaryval, 2);
                if (binaryval[0] == '1' && binaryval.Length == 8)
                {
                    bin = bin << 1;
                    binaryval = Convert.ToString(bin, 2);
                    if (binaryval.Length > 8)
                    {
                        binaryval = binaryval.Substring(1);
                    }
                    bin = Convert.ToInt32(binaryval, 2);
                    binaryval = Convert.ToString((xorb1int ^ bin), 2);
                    list.Add(binaryval);

                }
                else
                {
                    bin = bin << 1;
                    binaryval = Convert.ToString(bin, 2);
                    if (binaryval.Length > 8)
                    {
                        binaryval = binaryval.Substring(1);
                    }
                    bin = Convert.ToInt32(binaryval, 2);
                    list.Add(binaryval);
                }
            }
            string temp;
            if (constantMatrix == "0E")
            {
                list[0] = Convert.ToInt32(list[0], 2).ToString();
                for (int i = 1; i < list.Count; i++)
                {
                    list[i] = Convert.ToInt32(list[i], 2).ToString();
                    list[i] = (int.Parse(list[i]) ^ int.Parse(list[i - 1])).ToString();
                }
            }
            else if (constantMatrix == "0B")
            {
                list[0] = Convert.ToInt32(list[0], 2).ToString();
                list[2] = Convert.ToInt32(list[2], 2).ToString();
                temp = list[2];
                list[2] = (int.Parse(list[0]) ^ int.Parse(temp)).ToString();

            }
            else if (constantMatrix == "0D")
            {
                list[1] = Convert.ToInt32(list[1], 2).ToString();
                list[2] = Convert.ToInt32(list[2], 2).ToString();
                temp = list[2];
                list[2] = (int.Parse(list[1]) ^ int.Parse(temp)).ToString();

            }
            else if (constantMatrix == "09")
            {
                list[2] = Convert.ToInt32(list[2], 2).ToString();
            }


            retstring = Convert.ToString(Convert.ToInt32(list[list.Count - 1], 10), 2);

            if (b)
            {
                string hexast = Convert.ToString(Convert.ToInt32(retstring, 2), 16);
                retstring = XOR(hexast, cmatrixji);
                retstring = Convert.ToString(Convert.ToInt32(retstring, 16), 2);
            }
            return retstring;
        }

        public string[,] invMixcloumns(string[,] cmatrix)
        {
            string[,] arr = new string[4, 4];
            string[,] constantMat = new string[4, 4] { { "0E", "0B", "0D", "09" }, { "09", "0E", "0B", "0D" }, { "0D", "09", "0E", "0B" }, { "0B", "0D", "09", "0E" } };

            string[] tempres = new string[4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {

                        if (constantMat[i, k].Equals("0E"))
                        {
                            tempres[k] = invXorshift(cmatrix[k, j], false, "0E");
                        }
                        else if (constantMat[i, k].Equals("0B"))
                        {
                            tempres[k] = invXorshift(cmatrix[k, j], true, "0B");
                        }
                        else if (constantMat[i, k].Equals("0D"))
                        {
                            tempres[k] = invXorshift(cmatrix[k, j], true, "0D");
                        }
                        else
                        {
                            tempres[k] = invXorshift(cmatrix[k, j], true, "09");
                        }
                        tempres[k] = Convert.ToInt32(tempres[k], 2).ToString();
                    }
                    for (int k = 1; k < 4; k++)
                    {
                        tempres[k] = (int.Parse(tempres[k]) ^ int.Parse(tempres[k - 1])).ToString();


                    }
                    arr[i, j] = Convert.ToString(Convert.ToInt32(tempres[3], 10), 16).PadLeft(2, '0');
                }
            }

            return arr;

        }
        public override string Decrypt(string cipherText, string key)
        {
            List<string[,]> RoundKeyMatrix = keyExpansion(key);
            string[,] cipherMatrix = stringToMatrix(cipherText);
            string[,] initialRound = addRoundKey(cipherMatrix, RoundKeyMatrix[10]);
            string[,] result = initialRound;
            for (int i = 9; i >= 1; i--)
            {
                result = invShiftrows(result);
                result = inverssbox(result);
                result = addRoundKey(result, RoundKeyMatrix[i]);
                result = invMixcloumns(result);
            }
            result = invShiftrows(result);
            result = inverssbox(result);
            result = addRoundKey(result, RoundKeyMatrix[0]);
            string finalResult = matrixToString(result);
            return finalResult;
        }

        public override string Encrypt(string plainText, string key)
        {
            List<string[,]> RoundKeyMatrix = keyExpansion(key);
            string[,] plainMatrix = stringToMatrix(plainText);

            string[,] initialRound = addRoundKey(plainMatrix, RoundKeyMatrix[0]);
            string[,] result = initialRound;

            for (int i = 1; i <= 9; i++)
            {
                result = sbox(result);
                result = shiftrows(result);
                result = mixcloumns(result);
                result = addRoundKey(result, RoundKeyMatrix[i]);

            }
            result = sbox(result);
            result = shiftrows(result);
            result = addRoundKey(result, RoundKeyMatrix[10]);

            string finalResult = matrixToString(result);

            return finalResult;
        }
    }
}
