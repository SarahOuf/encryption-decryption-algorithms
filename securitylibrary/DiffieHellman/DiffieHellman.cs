using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
    {
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            List<int> keys = new List<int>();
            int ya, yb, ka, kb;
            ya = calc_fast_mod(q, alpha, xa);
            yb = calc_fast_mod(q, alpha, xb);
            ka = calc_fast_mod(q, yb, xa);
            kb = calc_fast_mod(q, ya, xb);
            keys.Add(ka);
            keys.Add(kb);
                return keys;
        }
        public int calc_fast_mod(int q,int alpha,int xa)
        {
            int[] powers_of_two = new int[100];
            string binaryXA = Convert.ToString(xa, 2);
            powers_of_two[0] = alpha % q;
            for (int i = 1; i < binaryXA.Length; i++)
            {
                powers_of_two[i] = (powers_of_two[i - 1] * powers_of_two[i - 1]) % q;
            }
            int pre_result = 1;
            for (int i = 0; i < binaryXA.Length; i++)
            {
                if (binaryXA[binaryXA.Length - 1 - i] == '1')
                {
                    pre_result *= powers_of_two[i];
                }
                pre_result = pre_result % q;
            }
           
            return pre_result;

        }
    }
}
