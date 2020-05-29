using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class Program
    {
        /// <summary>
        /// stores the rotor numbers
        /// </summary>
        static int[] rotorValues;
        /// <summary>
        /// Stores the rotor offset values
        /// </summary>
        static int[] rotorOffsets;
        /// <summary>
        /// Store the plugboard pairs
        /// </summary>
        static string plugboard;
        /// <summary>
        /// Stores the reflector
        /// </summary>
        static string reflector;

        static void Main(string[] args)
        {
            bool continueInput = true;

            do
            {
                Console.Clear();
                bool keepSettings = true;

                SetEnigmaSettings();

                do
                {
                    EnigmaSettings machine = new EnigmaSettings(rotorValues, rotorOffsets, reflector, plugboard);
                    //EnigmaSettings machine = new EnigmaSettings(new int[] { 2, 1, 0 }, new int[] { 0, 0, 0 }, Reflectors.ReflectorList[1], "ABCDEFGHIJKLMNOPQRST");
                    string output = "";

                    Console.Clear();
                    Console.WriteLine(machine.ToString());
                    Console.WriteLine("String to encrypt: ");
                    string input = Console.ReadLine().ToUpper();
                    RemoveSpaces(ref input);

                    Console.Clear();
                    Console.WriteLine("Input:\t" + input);

                    foreach (char c in input)
                        output += machine.DetermineValue(c);

                    Console.WriteLine("Output:\t" + output);
                    Console.WriteLine("\nPress any [r] to reset machine, Press [e] to continue with settings...");
                    string key = Console.ReadKey().KeyChar.ToString().ToUpper();
                    if (key == "R")
                        keepSettings = false;
                    else if (key == "E")
                    {
                        continueInput = false;
                        break;
                    }
                } while (keepSettings);
            } while (continueInput);

            Console.WriteLine("\n\nEnigma Machine Shutting Down........");
            Console.ReadKey();
        }

        /// <summary>
        /// Sets the machines settings
        /// </summary>
        /// <returns></returns>
        static EnigmaSettings SetEnigmaMachine()
        {
            return new EnigmaSettings(rotorValues, rotorOffsets, reflector, plugboard);
        }

        static void SetEnigmaSettings()
        {
            rotorValues = PromptForRotors();
            rotorOffsets = PromptForOffset();
            reflector = Reflectors.ReflectorList[ReflectorOption()];
            Console.WriteLine(reflector);
            Console.Write("Do you want plugboard settings?");
            if (Console.ReadKey().KeyChar.ToString().ToUpper() == "Y")
                plugboard = SetPlugboard();
        }

        /// <summary>
        /// Prompts for the rotors that will be used in the machine
        /// </summary>
        /// <returns></returns>
        static int[] PromptForRotors()
        {
            // the strings that define the rotors
            string[] rotorNames = { "RIGHT", "CENTER", "LEFT" };

            // the rotor values
            int[] numbers = new int[3];

            // runs through each rotor
            for (int i = 2; i >= 0; i--)
            {
                // determines if the input was valid
                bool valid = false;

                // prompts for a rotor number
                Console.Write(("ENTER NUMBER (1-8) FOR THE " + rotorNames[i] + " ROTOR :").PadRight(50));
                do 
                {
                    // if the value is a number
                    if (int.TryParse(Console.ReadLine(), out int num))
                    {
                        // if the value is in range
                        if (num >= 1 && num <= 8)
                        {
                            valid = true;
                            numbers[i] = --num;
                        }
                        // the value is out of range
                        else
                        {
                            Console.Write("INVALID NUMBER, PLEASE SELECT ANOTHER...");
                        }
                    }
                    // the value is not a number
                    else
                    {
                        Console.Write("INVALID NUMBER, PLEASE SELECT ANOTHER...");
                    }
                } while (!valid);
            }

            return numbers; // returns the values
        }

        /// <summary>
        /// Returns the inputted offset values
        /// </summary>
        /// <returns></returns>
        static int[] PromptForOffset()
        {
            // stores the values
            int[] values = new int[3];
            // determines if the input was valid
            bool valid = false;

            do
            {
                // prompts for the values
                Console.WriteLine("PLEASE ENTER THE OFFSET VALUES. MUST BE LETTERS SEPERATED BY SPACES: ");
                // splits the values into seperate indexes and flips the array
                string[] input = Console.ReadLine().Split(' ');
                ReverseArray(ref input);

                // if the array is not 3
                if (input.Length != 3)
                    Console.WriteLine("INVALID, PLEASE TRY AGAIN...");
                else
                {
                    valid = true;
                    // run through each letter and set the index accordingly
                    for (int i = 2; i >= 0 && valid; i--)
                    {
                        // stores the current letter
                        char current = input[i][0];
                        // if the letter is in range, convert to an int and store it
                        if (current >= 'A' && current <= 'Z')
                            values[i] = LetterToInt(current);
                        // otherwise, its invalid and display an error
                        else
                        {
                            valid = false;
                            Console.WriteLine("INVALID, PLEASE TRY AGAIN...");
                        }
                    }
                }
            } while (!valid);

            // return the value list
            return values;
        }

        /// <summary>
        /// Sets the desired reflector
        /// </summary>
        /// <returns></returns>
        static int ReflectorOption()
        {
            // the reflector options
            string[] options = { "A", "B", "C", "B Thin", "C Thin" };
            // the value that will be returned
            int value = -1;
            // the valid status
            bool valid = false;

            // prompts for a selection
            Console.Write("Select a reflector (A,B,C,B Thin, C Thin):\t");
            do
            {
                // the input
                string selection = Console.ReadLine();
                
                // checks each options to make sure its valid
                for (int i = 0; i < options.Length; i++)
                {
                    // if it matches
                    if (options[i] == selection)
                    {
                        valid = true;
                        value = i;
                    }
                }

                // if no match, display a prompt
                if (!valid)
                    Console.Write("INVALID, PLEASE ENTER ANOTHER...");
            } while (!valid);

            return value;
        }



        static string SetPlugboard()
        {
            string value = "";

            for (int i = 0; i < 10; i++)
            {
                bool valid = false;

                Console.Write("\nCOMBINATION {0}: ENTER 2 LETTERS:\t", i);
                do
                {
                    string input = Console.ReadLine();
                    if (input.Length != 2)
                        Console.WriteLine("INVALID, MUST BE 2 LETTERS..");
                    else if (input[0] == input[1])
                        Console.WriteLine("INVALID, CANNOT BE THE SAME LETTER...");
                    else if (input[0] < 'A' || input[0] > 'Z')
                        Console.WriteLine("INVALID, MUST BE A LETTER...");
                    else if (input[1] < 'A' || input[1] > 'Z')
                        Console.WriteLine("INVALID, MUST BE A LETTER...");
                    else if (value.Contains(input[1]))
                        Console.WriteLine("INVALID, LETTER ALREADY USED...");
                    else if (value.Contains(input[0]))
                        Console.WriteLine("INVALID, LETTER ALREADY USED...");
                    else
                    {
                        valid = true;
                        value += input;
                    }
                } while (!valid);
            }

            return value;
        }



        static void RemoveSpaces(ref string input)
        {
            string[] str = input.Split(' ');

            input = str[0];
            for (int i = 1; i < str.Length; i++)
                input += str[i];
        }









        /// <summary>
        /// Converts the letter to an integer
        /// </summary>
        /// <param name="l">the letter</param>
        /// <returns>the corresponding int</returns>
        static int LetterToInt(char l){ return l - (int)'A';  }

        /// <summary>
        /// Refereses the array
        /// </summary>
        /// <param name="a">the array to reverse</param>
        static void ReverseArray(ref string[] a)
        {
            string[] temp = new string[a.Length];

            for (int i = 0; i < a.Length; i++)
                temp[i] = a[a.Length - 1 - i];

            a = temp;
        }
    }
}
