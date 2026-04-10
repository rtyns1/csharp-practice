using System;

namespace ReverseString
{
    public class StringReverser
    {
        public void Run()
        {
            Console.Write("Enter a string: ");
            string input = Console.ReadLine();

            string reversed = ReverseString(input);

            Console.WriteLine("Reversed string: " + reversed);
        }

        private string ReverseString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            char[] result = new char[input.Length];
            int j = 0;

            for (int i = input.Length - 1; i >= 0; i--)
            {
                result[j] = input[i];
                j++;
            }

            return new string(result);
        }
    }
}
