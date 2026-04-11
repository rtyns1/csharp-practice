using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;



namespace ArrayMinMax
{
    public class ArrayMinMax
    {
        //first step as always is to aim to understand the problem
        // so we take user input, make sure they are numeric, then put them in an array
        // then we find the min and max of the array and print them out
        public void GetMinMax()
        {
            Console.WriteLine("Enter a list of numbers separated by commas: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input cannot be empty.Try again.");
                return;
            }


            string[] numbers = input.Split(',');
            List<int> validNumbers = new List<int>();
            bool allValid = true;

            foreach (string number in numbers)
            {
                if (int.TryParse(number.Trim(), out int validNumber))
                {
                    validNumbers.Add(validNumber);
                }
                else
                {
                    allValid = false;
                    Console.WriteLine($"'{number.Trim()}' is not a valid number.");
                }

            }

            int[] Numbers = validNumbers.ToArray();

            int max = Numbers[0];
            int min = Numbers[0];

            for (int i = 0; i < Numbers.Length; i++)
            {
                if (Numbers[i] > max)
                {
                    max = Numbers[i];
                }

                if (Numbers[i] < min)
                {
                    min = Numbers[i];

                }

            }


            Console.WriteLine($"The maximum number is {max}");
            Console.WriteLine($"The minimum number is {min}");



        }


    }
}
