﻿using System;

int value = Random.Shared.Next(1, 101);
while (true)
{
	Console.Write("Guess a number (1-100): ");
	bool valid = int.TryParse((Console.ReadLine() ?? "").Trim(), out int input);

	if (!valid)
	{
		Console.WriteLine("Invalid. Please enter a number.");
	}
	else if (input < 1 || input > 100)
	{
		Console.WriteLine("Invalid. Number must be between 1 and 100.");
	}
	else if (input == value)
	{
		break;
	}
	else
	{
		Console.WriteLine($"Incorrect. Too {(input < value ? "Low" : "High")}.");
	}
}
Console.WriteLine("You guessed it!");
Console.Write("Press any key to exit...");
Console.ReadKey(true);
