using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Connect_4_Game;

// HOW I WANT TO  DO IT HAAAAAAAAAAAAAAAAAAAAAA
// (ABSTRACT) GAME CLASS |
// ----------------------
// + Start (): Void 
// + PlayTurno (): void
// + IsGameOver (): bool 
// + GetWinner (): string
// 
// ConnectFourGame CLASS |
// ----------------------
//  - Board: board
//  - Players: Player[] 
//  - CurrentPlayer : Player
//  - turnCounter: int
// ----------------------
//  + Start(): void 
//  + PlayTurn(): void
//  + IsGameOver(): bool
//  + GetWinner(): string
//  - SwitchPlayers():void
//
//  Board CLASS |
// -----------------
//  - grid: Cell[,]
// ----------------- 
//  + Initialize (): void
//  + DropPiece(col,sym): bool
//  + Display(): void
//  + CheckWin(sym): bool 
//  + IsFull(): bool
//
//  Cell CLASS |
//--------------
//  - symbol: char
// ----------------
//  + GetSymbol(): char
//  + SetSymbol(c): void
//
//  (ABSTRACT) Player CLASS |  
//-------------------------
//  #name : string
//  # symbol: char
//--------------------------
//  + GetMove(board): int
//  + Name {get; set;}
//  + Symbol {get; set;}
//
//  HumanPlayer CLASS |
//--------------------
//  + GetMove(board): int


public class Board
{
    private const int Rows = 6;
    private const int Columns = 7;
    private char[,] Grid = new char[Rows, Columns];


    // this method fills the board with space ' ' to start the game clean

    public void Initialize()// this Method will call at the beginning of the game 
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                Grid[row, col] = ' '; // grid is a 2D array 
            }
        }

    }

    // this part will be the display the visual game

    public void Display()
    {
        {
            Console.WriteLine();
            Console.WriteLine("   CONNECT FOUR");
            Console.WriteLine();

            for (int row = 0; row < 6; row++)
            {
                Console.Write(" | ");
                for (int col = 0; col < 7; col++)
                {
                    Console.Write(Grid[row, col]);
                    Console.Write(" | ");
                }
                Console.WriteLine();
            }

            Console.WriteLine(" -----------------------------");
            Console.WriteLine("   1   2   3   4   5   6   7\n");
        }
    }

    //  this method will be the drop a piece into a Column 

    public bool DropPiece(int column, char symbol) // bool because need to check true or false of position 
    {
        column -= 1; // adjust for 0 based array 

        if (column < 0 || column >= Columns)
            
            return false;

        for (int row = Rows -1; row >= 0; row--)
        {
            if (Grid[row, column] == ' ')
            {
                Grid[row, column] = symbol;
                
                return true; 
            }
        }

        return false; // column is full 
    }

    // this part will check if the board is full "tie" 

    public bool IsFull()
    {
        for (int col = 0; col < Columns; col++)
        {
            if (Grid[0, col] == ' ')

                return false;
        }

        return true;
    }

    //This part will check the directions 4 in row 

    public bool CheckWin(char symbol)
    {
        // this is horizontal check
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col <= Columns - 4; col++)
            {
                if (Grid[row, col] == symbol && Grid[row, col + 1] == symbol & Grid[row, col + 2] == symbol && Grid[row, col + 3] == symbol)
                    return true;
            }
        }

        // this is vertical check

        for (int col = 0; col < Columns; col++)
        {
            for (int row = 0; row <= Rows - 4; row++)
            {
                if (Grid[row, col] == symbol && Grid[row + 1, col] == symbol & Grid[row + 2, col] == symbol && Grid[row + 3, col] == symbol)
                    return true;
            }
        }

        // this part is diagonal (down-right)

        for (int row = 0; row <= Rows - 4; row++)
        {
            for (int col = 0; col <= Columns - 4; col++)
            {
                if (Grid[row, col] == symbol && Grid[row + 1, col + 1] == symbol & Grid[row + 2, col + 2] == symbol && Grid[row + 3, col +3] == symbol)
                    return true;
            }
        }

        // this is the other diagonal up-right

        for (int row = 3; row < Rows; row++)
        {
            for (int col = 0; col <= Columns - 4; col++)
            {
                if (Grid[row, col] == symbol && Grid[row - 1, col + 1] == symbol & Grid[row - 2, col + 2] == symbol && Grid[row - 3, col + 3] == symbol)
                    return true;

            }
        }

        return false;
    }

}

public abstract class Player
{
    public string Name { get; set; } 
    public char Symbol { get; set; } // 'X' OR 'O'

    public Player (string name, char symbol)
    {
        Name = name;
        Symbol = symbol;
    }
    public abstract int GetMove(Board board); //each player type can give their own logic for choosing a column
}

public class HumanPlayer : Player
{
    public HumanPlayer(string name, char symbol) : base(name,symbol)
    {

    }

    public override int GetMove(Board board)
    {
        int column;

        while(true)
        {
            Console.Write($"{Name} ({Symbol}), choose a column (1-7): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out column) && column >= 1 && column <=7)
            {
                return column;
            }

            Console.WriteLine("Invalid input.Please enter a number from 1 to 7.");
        }

    }
}

// This is the "Guide" for what every game needs to do 
public abstract class Game
{
    public abstract void StartGame();
    public abstract void PlayTurn();
    public abstract bool IsGameOver();
    public abstract string GetWinner();
}

//This is the type of Game 
public class ConnectFourGame : Game
{
    public Board Board { get; set; }
    public Player[] Players { get; set; }
    public Player CurrentPlayer { get; set; }
    public int TurnCounter { get; set; }



    public ConnectFourGame()
    {
        Board = new Board();
        Players = new Player[2];
        TurnCounter = 0;
    }

    public override void StartGame()
    {
        Console.Clear(); // clear screen for clean start

        Console.WriteLine(@" 
          ____                            _     _  _      ____                      
         / ___|___  _ __  _ __   ___  ___| |_  | || |    / ___| __ _ _ __ ___   ___ 
        | |   / _ \| '_ \| '_ \ / _ \/ __| __| | || |_  | |  _ / _` | '_ ` _ \ / _ \
        | |__| (_) | | | | | | |  __/ (__| |_  |__   _| | |_| | (_| | | | | | |  __/
         \____\___/|_| |_|_| |_|\___|\___|\__|    |_|    \____|\__,_|_| |_| |_|\___|
        ");

        Players[0] = new HumanPlayer("Player 1", 'X');
        Players[1] = new HumanPlayer("Player 2", 'O');

        CurrentPlayer = Players[0];

        Board.Initialize(); // clear the grid (board)

        Console.WriteLine("Welcome to Connect Four!! :D");
    }

    public override void PlayTurn()
    {
        Console.WriteLine($"\nTurn {TurnCounter + 1} — {CurrentPlayer.Name}'s move:");

        Board.Display();//show the board of game 

        int column = CurrentPlayer.GetMove(Board);// ask player the position 

        bool success = Board.DropPiece(column, CurrentPlayer.Symbol);

        if (success)
        {
            TurnCounter++;

            if (!IsGameOver())
            {
                SwitchPlayers(); 
            }
        }
        else
        {
            Console.WriteLine("That column is full =(! Try again! :D");
        }
    }

    public override bool IsGameOver()
    {
        return Board.CheckWin(CurrentPlayer.Symbol) || Board.IsFull();
    }
    public override string GetWinner()
    {
        if (Board.CheckWin(CurrentPlayer.Symbol))

            return CurrentPlayer.Name;

        return "Tie!!!!";
    }
    private void SwitchPlayers()
    {
        if (CurrentPlayer == Players[0])

            CurrentPlayer = Players[1];

        else

            CurrentPlayer = Players[0]; 
    }
}

class Program
{
    static void Main(string[] args)
    {
        bool playAgain = true;

        while (playAgain)
        {
            ConnectFourGame game = new ConnectFourGame();
            game.StartGame();

            while (!game.IsGameOver())
            {
                game.PlayTurn();
            }

            Console.Clear();
            game.Board.Display();

            string winner = game.GetWinner();

            if (winner == "Tie")
                Console.WriteLine("It's a tie!!! =X");
            else
                Console.WriteLine($"{winner} wins!");

            Console.Write("Do you want to play again? (y/n): ");
            string answer = Console.ReadLine().ToLower();

            playAgain = (answer == "y");

        }

        Console.WriteLine("Thanks for playing :D!");
    }
      
    
}




