using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public static int Ext_Euclidean(int A2, int A3, int B2, int B3)
        {
            if (B3 == 0)
            {
                return -1;
            }
            if (B3 == 1)
            {
                return B2;
            }
            int Q = A3 / B3;

            int T2 = A2 - (Q * B2);
            int T3 = A3 - (Q * B3);
            A2 = B2;
            A3 = B3;
            B2 = T2;
            B3 = T3;
            return Ext_Euclidean(A2, A3, B2, B3);
        }

        public int GetMultiplicativeInverse(int number, int baseN)
        {
            //throw new NotImplementedException();
            int result = Ext_Euclidean(0, baseN, 1, number);
            if (result < 0 && result != -1)
            {
                result = (((result % baseN) + baseN) % baseN);
            }
            return result;
        }
    }
}
