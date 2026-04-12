# Array Min/Max Finder

## Description
A console application that prompts the user for a comma-separated list of numbers and returns the maximum and minimum values.

## How to Use
1. Run the application.
2. Enter numbers separated by commas (e.g., `78,45,100,23`).
3. The program outputs the largest and smallest number.

## How It Works
The input is read as a string, split by commas, and each part is converted to an integer. Invalid entries are reported but skipped.
The first valid number initializes the min and max variables. A single loop compares each subsequent number, 
updating min or max when a smaller or larger value is found.

## Edge Cases Handled
- Empty input → error message, program exits gracefully
- Invalid numbers like `abc` → warning message, number skipped
- Single number → that number is both min and max
- Spaces around commas → automatically trimmed

## Technologies Used
- .NET 8.0
- C# 12.0

## Author
Your Name