using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class EnigmaSettings
    {
        /// <summary>
        /// The number of rotors
        /// </summary>
        const int NUMBER_OF_ROTORS = 3;

        /// <summary>
        /// the max number of combinations that can be made in the plugboard
        /// </summary>
        const int MAX_NUMBER_OF_PLUGBOARD = 10;

        /// <summary>
        /// Stores the rotors
        /// </summary>
        Rotor[] rotors = new Rotor[NUMBER_OF_ROTORS];

        string plugboard;
        bool usingPlugboard;

        string reflector;

        /// <summary>
        /// Initializes a set of settings for the inigma machine
        /// </summary>
        /// <param name="indexes"></param>
        /// <param name="offsets"></param>
        public EnigmaSettings(int[] indexes, int[] offsets, string reflector, string pb = "")
        {
            SetEnigmaSettings(indexes, offsets, reflector, pb);
        }

        /// <summary>
        /// Sets the enigma machine settings
        /// </summary>
        /// <param name="indexes"></param>
        /// <param name="offsets"></param>
        public void SetEnigmaSettings(int[] indexes, int[] offsets, string reflector, string pb = "")
        {
            plugboard = "";
            usingPlugboard = false;

            // if an invalid number of values were entered
            if (indexes.Length != NUMBER_OF_ROTORS || offsets.Length != NUMBER_OF_ROTORS)
                throw new IndexOutOfRangeException("INVALID");

            // otherwise the number was valid
            else
            {
                // initialize a new rotor based on the values
                for (int i = 0; i < indexes.Length; i++)
                    rotors[i] = new Rotor(indexes[i], offsets[i]);
            }

            // if the plugboard is not set
            if (pb == "" || pb == null)
                usingPlugboard = false;
            // if the plugboard is set, save the settings
            else
            {
                usingPlugboard = true;
                SetPlugboard(pb);
            }

            // sets the reflector
            this.reflector = reflector;
        }

        /// <summary>
        /// Determine the new encrypted value
        /// </summary>
        /// <param name="input">the letter that was entered</param>
        /// <returns>the encrypted value</returns>
        public char DetermineValue(char input)
        {
            // rotates the first rotor
            rotors[0].RotateCipher();

            // if the first rotor initiated the second rotor to spin
            if (CheckToAdvance(rotors[0]))
            {
                // rotate the second rotor
                rotors[1].RotateCipher();
                
                // if the second initiated the third rotor to spin
                if (CheckToAdvance(rotors[1]))
                    rotors[2].RotateCipher();
            }

            // stores the alphabet string
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string reflect = reflector;

            int index;
            char letter = input;

            index = alphabet.IndexOf(letter);
            PlugboardSettings(ref letter, ref index);

            for (int r = 0; r <= rotors.Length - 1; r++)
                RightToLeft(ref letter, ref index, rotors[r]);

            letter = reflect[index];
            index = alphabet.IndexOf(letter);
            letter = alphabet[index];

            for (int r = rotors.Length - 1; r >= 0; r--)
                LeftToRight(ref letter, ref index, rotors[r]);

            letter = alphabet[index];
            PlugboardSettings(ref letter, ref index);

            // return the final letter
            return letter;
        }

        /// <summary>
        /// Checks the letter in the Cipher and the index in the alphabet
        /// </summary>
        /// <param name="l">the stored letter</param>
        /// <param name="i">the stored index</param>
        /// <param name="r">the rotor to check</param>
        private void RightToLeft(ref char l, ref int i, Rotor r)
        {
            l = r.LetterInCipher(i);
            i = r.AlphaIndexOfLetter(l);
        }

        /// <summary>
        /// Checks the letter in the alphabet and the index of the cipher
        /// </summary>
        /// <param name="l">the stored letter</param>
        /// <param name="i">the stored index</param>
        /// <param name="r">the rotor to check</param>
        private void LeftToRight(ref char l, ref int i, Rotor r)
        {
            l = r.LetterInAlpha(i);
            i = r.CipherIndexOfLetter(l);
        }

        /// <summary>
        /// Checks if the rotor needs to be advanced
        /// </summary>
        /// <param name="r">the previous rotor</param>
        /// <returns></returns>
        private bool CheckToAdvance(Rotor r)
        {
            // stores the list of values that rotate the rotor
            List<char> values = RotorInformation.RotateValues(r.RotorIndex());

            // if the list contains the previous letter, return true
            if (values.Contains(r.LetterInAlpha(25)))
                return true;

            // does not need to be rotated
            return false;
        }

        /// <summary>
        /// Returns the machine as a string.
        /// STORES the threee rotor settings
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "CURRENT MACHINE:";
            str += "\n===========================================\n";
            str += "Rotors:\t(" + rotors[0].RotorIndex() + ", " + rotors[1].RotorIndex() + ", " + rotors[2].RotorIndex() + ")\n";
            str += rotors[0].ToString() + "\n" + rotors[1].ToString() + "\n" + rotors[2].ToString();
            str += "\n-------------------------------------------\n";
            str += "Reflector:\t" + reflector;

            if (plugboard.Length > 0)
            {
                str += "\n-------------------------------------------";
                str += "\nPlugboard:";

                for (int i = 0; i < plugboard.Length; i += 2)
                    str += "\n\t\t" + plugboard[i] + " <--> " + plugboard[i + 1];
            }

            str += "\n===========================================\n";

            return str;
        }




        private void SetPlugboard(string pb)
        {
            plugboard = pb;
        }

        private void PlugboardSettings(ref char l, ref int i)
        {
            if (usingPlugboard && plugboard.Contains(l))
            {
                bool letterFound = false;
                for (int index = 0; index < plugboard.Length && !letterFound; index += 2)
                {
                    char first = plugboard[index];
                    char second = plugboard[index + 1];

                    if (l == first)
                    {
                        l = second;
                        letterFound = true;
                        i = (int)second - (int)'A';
                    }
                    else if (l == second)
                    {
                        l = first;
                        letterFound = true;
                        i = (int)first - (int)'A';
                    }
                }
            }
        }
    }
}
