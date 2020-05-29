using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class RotorInformation
    {
        /// <summary>
        /// The wiring connected to ABC...XYZ
        /// </summary>
        public static readonly string[] ROTOR_CYPERS =
        {
            "EKMFLGDQVZNTOWYHXUSPAIBRCJ",
            "AJDKSIRUXBLHWTMCQGZNPYFVOE",
            "BDFHJLCPRTXVZNYEIWGAKMUSQO",
            "ESOVPZJAYQUIRHXLNFTGKDCMWB",
            "VZBRGITYUPSDNHLXAWMJQOFECK",
            "JPGVOUMFYQBENHZRDKASXLICTW",
            "NZJHGRCXMYSWBOUFAIVLPEKQDT",
            "FKQHTLXOCBJSPDZRAMEWNIUYGV",
        };

        /// <summary>
        /// The values that advance the next rotor
        /// NOTE: Last letter is for rotors VI (5) to VIII (7)
        /// </summary>
        private static readonly char[] ROTOR_ADVANCES = { 'Q', 'E', 'V', 'J', 'Z', 'Z', 'Z', 'Z', 'M' };

        /// <summary>
        /// Retrieve the letters that advance the next rotor
        /// </summary>
        /// <param name="i">the index of the rotor in the array</param>
        /// <returns></returns>
        public static List<char> RotateValues(int i)
        {
            List<char> values = new List<char>();   // create a list
            values.Add(ROTOR_ADVANCES[i]);          // store the appropriate value

            // if the rotor has an additional advance letter, add it
            if (i >= 5 && i <= 7)
                values.Add(ROTOR_ADVANCES[8]);

            // return the values
            return values;
        }
    }
}
