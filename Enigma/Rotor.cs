using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    public class Rotor
    {
        /// <summary>
        /// Stores the cypher
        /// </summary>
        private string cipher;
        /// <summary>
        /// The index of the original cipher
        /// </summary>
        private int cipherIndex;
        /// <summary>
        /// The alphabet string
        /// </summary>
        private string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Initializes the rotor
        /// </summary>
        /// <param name="rotorIndex">the desired cypher</param>
        /// <param name="offset">the beginning index of the cypher</param>
        public Rotor(int rotorIndex = 0, int offset = 0)
        {
            cipher = RotorInformation.ROTOR_CYPERS[rotorIndex];
            cipherIndex = rotorIndex;
            RotateCipher(offset);
        }

        /// <summary>
        /// The current order of the rotor
        /// </summary>
        /// <returns></returns>
        public string RotorOrder() { return cipher; }

        /// <summary>
        /// Rotates the cypher and sets the string appropriately
        /// </summary>
        /// <param name="index">the index to rotate to</param>
        public void RotateCipher(int index = 1)
        {
            if (index > 0 || index < cipher.Length)
            {
                cipher = Resort(cipher, index);
                alpha = Resort(alpha, index);
            }
        }

        /// <summary>
        /// Advances the ciper to the appropriate location
        /// </summary>
        /// <param name="s">the cipher to be advanced</param>
        /// <param name="i">the index to advance it to</param>
        /// <returns>the advanced string</returns>
        private string Resort(string s, int i) { return s.Substring(i, cipher.Length - i) + s.Substring(0, i); }
        
        /// <summary>
        /// Retrieves the letter at the index of the cipher
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char LetterInCipher(int index) { return cipher[index]; }
        /// <summary>
        /// Retrieves the letter at the index of the alphabet
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char LetterInAlpha(int index) { return alpha[index]; }

        /// <summary>
        /// Retrieves the index of the letter in the cipher
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public int CipherIndexOfLetter(char letter) { return cipher.IndexOf(letter); }
        /// <summary>
        /// Retrieves the index of the letter in the alphabet
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public int AlphaIndexOfLetter(char letter) { return alpha.IndexOf(letter); }

        /// <summary>
        /// Returnes the number of the rotor
        /// </summary>
        /// <returns></returns>
        public int RotorIndex() { return cipherIndex; }

        /// <summary>
        /// Converts the rotor to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "\tCypher:\t" + cipher + "\n\tAplha:\t" + alpha;
        }
    }
}
