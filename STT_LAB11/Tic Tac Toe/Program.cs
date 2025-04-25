using System;
using System.Collections.Generic;

bool closeRequested = false;
bool playerTurn = true; // This will be set based on player choice now
char[,] board;

while (!closeRequested)
{
	board = new char[3, 3]
	{
		{ ' ', ' ', ' ', },
		{ ' ', ' ', ' ', },
		{ ' ', ' ', ' ', },
	};

	// Add player choice for who starts first
	playerTurn = ChooseFirstPlayer();

	while (!closeRequested)
	{
		if (playerTurn)
		{
			PlayerTurn();
			if (CheckForThree('X'))
			{
				EndGame("  You Win.");
				break;
			}
		}
		else
		{
			ComputerTurn();
			if (CheckForThree('O'))
			{
				EndGame("  You Lose.");
				break;
			}
		}
		playerTurn = !playerTurn;
		if (CheckForFullBoard())
		{
			EndGame("  Draw.");
			break;
		}
	}
	if (!closeRequested)
	{
		Console.WriteLine();
		Console.WriteLine("  Play Again [enter], or quit [escape]?");
	GetInput:
		Console.CursorVisible = false;
		switch (Console.ReadKey(true).Key)
		{
			case ConsoleKey.Enter: break;
			case ConsoleKey.Escape:
				closeRequested = true;
				Console.Clear();
				break;
			default: goto GetInput;
		}
	}
}
Console.CursorVisible = true;

bool ChooseFirstPlayer()
{
	Console.Clear();
	Console.WriteLine("  Tic Tac Toe\n");
	Console.WriteLine("  Who should go first?");
	Console.WriteLine("  [Y] You");
	Console.WriteLine("  [C] Computer");

	while (true)
	{
		switch (Console.ReadKey(true).Key)
		{
			case ConsoleKey.Y:
				return true;
			case ConsoleKey.C:
				return false;
			case ConsoleKey.Escape:
				closeRequested = true;
				Console.Clear();
				return true; // Default to player if escaping
		}
	}
}

void PlayerTurn()
{
	var (row, column) = (0, 0);
	bool moved = false;
	while (!moved && !closeRequested)
	{
		Console.Clear();
		RenderBoard();
		Console.WriteLine();
		Console.WriteLine("  Use the arrow and enter keys to select a move.");
		Console.SetCursorPosition(column * 4 + 4, row * 2 + 4);
		Console.CursorVisible = true;
		switch (Console.ReadKey(true).Key)
		{
			case ConsoleKey.UpArrow: row = row <= 0 ? 2 : row - 1; break;
			case ConsoleKey.DownArrow: row = row >= 2 ? 0 : row + 1; break;
			case ConsoleKey.LeftArrow: column = column <= 0 ? 2 : column - 1; break;
			case ConsoleKey.RightArrow: column = column >= 2 ? 0 : column + 1; break;
			case ConsoleKey.Enter:
				if (board[row, column] is ' ')
				{
					board[row, column] = 'X';
					moved = true;
				}
				break;
			case ConsoleKey.Escape:
				Console.Clear();
				closeRequested = true;
				break;
		}
	}
}

void ComputerTurn()
{
	// Implement optimal AI strategy for computer
	var move = FindBestMove();
	board[move.X, move.Y] = 'O';
}

(int X, int Y) FindBestMove()
{
	// Check if computer can win in one move
	for (int i = 0; i < 3; i++)
	{
		for (int j = 0; j < 3; j++)
		{
			if (board[i, j] == ' ')
			{
				board[i, j] = 'O';
				if (CheckForThree('O'))
				{
					board[i, j] = ' '; // Reset
					return (i, j);
				}
				board[i, j] = ' '; // Reset
			}
		}
	}

	// Check if player can win in one move, block it
	for (int i = 0; i < 3; i++)
	{
		for (int j = 0; j < 3; j++)
		{
			if (board[i, j] == ' ')
			{
				board[i, j] = 'X';
				if (CheckForThree('X'))
				{
					board[i, j] = ' '; // Reset
					return (i, j);
				}
				board[i, j] = ' '; // Reset
			}
		}
	}

	// Take center if available
	if (board[1, 1] == ' ')
	{
		return (1, 1);
	}

	// Take corners if available
	var corners = new List<(int X, int Y)>
	{
		(0, 0), (0, 2), (2, 0), (2, 2)
	};

	foreach (var corner in corners)
	{
		if (board[corner.X, corner.Y] == ' ')
		{
			return corner;
		}
	}

	// Take any available side
	var sides = new List<(int X, int Y)>
	{
		(0, 1), (1, 0), (1, 2), (2, 1)
	};

	foreach (var side in sides)
	{
		if (board[side.X, side.Y] == ' ')
		{
			return side;
		}
	}

	// Fallback to first available move (should not happen with proper board checking)
	for (int i = 0; i < 3; i++)
	{
		for (int j = 0; j < 3; j++)
		{
			if (board[i, j] == ' ')
			{
				return (i, j);
			}
		}
	}

	// This should never happen if CheckForFullBoard is correct
	return (0, 0);
}

bool CheckForThree(char c) =>
	board[0, 0] == c && board[1, 0] == c && board[2, 0] == c ||
	board[0, 1] == c && board[1, 1] == c && board[2, 1] == c ||
	board[0, 2] == c && board[1, 2] == c && board[2, 2] == c ||
	board[0, 0] == c && board[0, 1] == c && board[0, 2] == c ||
	board[1, 0] == c && board[1, 1] == c && board[1, 2] == c ||
	board[2, 0] == c && board[2, 1] == c && board[2, 2] == c ||
	board[0, 0] == c && board[1, 1] == c && board[2, 2] == c ||
	board[2, 0] == c && board[1, 1] == c && board[0, 2] == c;

bool CheckForFullBoard() =>
	board[0, 0] != ' ' && board[1, 0] != ' ' && board[2, 0] != ' ' &&
	board[0, 1] != ' ' && board[1, 1] != ' ' && board[2, 1] != ' ' &&
	board[0, 2] != ' ' && board[1, 2] != ' ' && board[2, 2] != ' ';

void RenderBoard()
{
	Console.WriteLine($"""

          Tic Tac Toe

          ╔═══╦═══╦═══╗
          ║ {board[0, 0]} ║ {board[0, 1]} ║ {board[0, 2]} ║
          ╠═══╬═══╬═══╣
          ║ {board[1, 0]} ║ {board[1, 1]} ║ {board[1, 2]} ║
          ╠═══╬═══╬═══╣
          ║ {board[2, 0]} ║ {board[2, 1]} ║ {board[2, 2]} ║
          ╚═══╩═══╩═══╝
        """);
}

void EndGame(string message)
{
	Console.Clear();
	RenderBoard();
	Console.WriteLine();
	Console.Write(message);
}
