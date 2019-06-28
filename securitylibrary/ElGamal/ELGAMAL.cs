using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityLibrary.AES;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        public int calculateMod(int number, int power, int baseN)
        {
            if (power == 0)
            {
                return 1;
            }
            if (number == 0)
            {
                return 0;
            }

            // If power is even 
            int result;
            if (power % 2 == 0)
            {
                result = calculateMod(number, power / 2, baseN);
                result = (result * result) % baseN;
            }
            else     // If power is odd 
            {
                result = number % baseN;
                result = (result * calculateMod(number, power - 1, baseN) % baseN) % baseN;
            }

            return (int)((((result % baseN) + baseN) % baseN));
        }
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            //throw new NotImplementedException();
            List<long> results = new List<long>();
          
            long c1 = calculateMod(alpha, k, q);
            long key = calculateMod(y, k, q);
            long c2 = (key * m) % q;

            results.Add(c1);
            results.Add(c2);

            return results;

        }
        public int Decrypt(int c1, int c2, int x, int q)
        {
            //throw new NotImplementedException();
            ExtendedEuclid algorithm = new ExtendedEuclid();          
            int k = calculateMod(c1, x, q);
            Console.WriteLine(k);
            Console.WriteLine(algorithm.GetMultiplicativeInverse(k, q));
            int plaintext = (c2 * algorithm.GetMultiplicativeInverse(k, q)) % q;

            return plaintext;

        }
    }
}
