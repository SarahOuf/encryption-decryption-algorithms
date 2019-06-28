using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {

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
        public static long fast_pwr(long Base, long exponent, long mod)
        {
            if (exponent == 0)
            {
                return 1;
            }
            long Res = fast_pwr((Base * Base) % mod, exponent / 2, mod) % mod;
            if (exponent % 2 == 1)
            {
                Res *= Base;
            }
            return Res % mod;
        }

        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q;
            int totient = (p - 1) * (q - 1);

            double C_t = fast_pwr(M, e, n);
            return (int)C_t;

        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int N = p * q;
            int Totient_N = (p - 1) * (q - 1);
            int D = Ext_Euclidean(0, Totient_N, 1, e);
            D = ((D % Totient_N) + Totient_N) % Totient_N;
            int M = (int)fast_pwr(C, D, N);
            return M;
        }
    }
}
