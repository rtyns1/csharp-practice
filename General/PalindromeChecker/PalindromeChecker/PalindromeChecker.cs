using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace PalindromeChecker
{
    public class PalindromeChecker
    {
        // this is an algorithm that is a symmetry check, it checks if the first and last characters are the same, then moves inwards, one checker moving left one cehcker moving right until they meet at the middle
        //if they are the same, then it is a palindrome
        // we also need to know its time complexity, wich is 0(n) because we have to check each character in the string atleast once, and space complexity is 0(1) because we are only using a few variables to keep track of the indices and the characters we are comparing
        public void Run()
        {
            Console.WriteLine("Enter string: ");
            string? input = Console.ReadLine();
            string? cleanedinput = new string(input.Where(char.IsLetterOrDigit).Select(char.ToLower).ToArray());

            //frmo this point, we deal with the algorithmic side of things, we will use 2 pointes, and a while loop, 

            bool ispalindrome = true;
            int left = 0;
            int right = cleanedinput.Length - 1;

            while (right > left)
            {
                if (cleanedinput[left] != cleanedinput[right])
                {
                    ispalindrome = false;
                    break;
                }
                left++;
                right--;
                // we already assume true, so we are basically done
            }

            Console.WriteLine($"Is palindrome: {ispalindrome}");

        }

    } 
}
