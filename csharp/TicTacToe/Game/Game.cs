using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TicTacToe.Game
{
    
    public enum Turn
    {
        XTURN,
        OTURN
    }

    public enum Status
    {
        XWIN,
        OWIN,
        DRAW,
        ONGOING
    }

    class Game
    {
        private static string playerXname, playerOname;
        private static Turn turn;
        private static List<string> board;



        public void Setup()
        {
            Console.WriteLine(@"Welcome to TicTacToe!
Our rules are simple:
	- Each square on the board is numbered from 1 to 9.
	- When it is your turn, you enter the number of the square you want to play at
	- That's it! You expected more! Silly! Haha.
	");
            Console.Write("Enter playerX name: ");
            playerXname = Console.ReadLine();
            Console.Write("Enter PlayerO name: ");
            playerOname = Console.ReadLine();
            turn = Turn.XTURN;
            board = Enumerable.Range(1, 9).Select(i => i.ToString()).ToList();
            DrawBoard();
        }

        public void Start()
        {
            while (true)
            {
                Console.Write(MakePrompt(turn));
                string move = Console.ReadLine();
                if (!int.TryParse(move, out int num))
                {
                    Console.WriteLine("Not a valid number!");
                }
                else if (num < 1 || num > 9)
                {
                    Console.WriteLine("Invalid move!");
                }
                else
                {
                    num -= 1;
                    if (MakeMove(num))
                    {
                        DrawBoard();
                        SwitchTurns(ref turn);
                    }
                    else
                    {
                        Console.WriteLine("Make a legal move!");
                    }
                }

                switch (CheckGameStatus())
                {
                    case Status.XWIN:
                        Console.WriteLine($"{playerXname} has won!");
                        Environment.Exit(0);
                        break;
                    case Status.OWIN:
                        Console.WriteLine($"{playerOname} has won!");
                        Environment.Exit(0);
                        break;
                    case Status.DRAW:
                        Console.WriteLine("It's a draw!");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public string GetPlayer(Turn turn)
        {
            return turn == Turn.XTURN ? playerXname : playerOname;
        }

        public string MakePrompt(Turn turn)
        {
            return $"{GetPlayer(turn)} >> ";
        }

        public void SwitchTurns(ref Turn turn)
        {
            turn = turn == Turn.XTURN ? Turn.OTURN : Turn.XTURN;
        }

        public bool MakeMove(int move)
        {
            string moveChar = turn == Turn.XTURN ? "X" : "O";
            List<int> legalMoves = FindLegalMovesIndices();
            if (legalMoves.Contains(move))
            {
                board[move] = moveChar;
                return true;
            }
            else
            {
                Console.WriteLine("Not a legal move!");
                return false;
            }
        }

        public List<int> FindLegalMovesIndices()
        {
            List<int> legal = new List<int>();
            Regex regex = new Regex("[0-9]");
            for (int i = 0; i < board.Count; i++)
            {
                if (regex.IsMatch(board[i]))
                {
                    legal.Add(i);
                }
            }
            return legal;
        }

        public void DrawBoard()
        {
            string prettyBoard = $"{board[0]} | {board[1]} | {board[2]}\n{board[3]} | {board[4]} | {board[5]}\n{board[6]} | {board[7]} | {board[8]}";
            Console.WriteLine(prettyBoard);
        }

        public bool CheckWin(string playerChar)
        {
            return (board[0] == playerChar && board[1] == playerChar && board[2] == playerChar) ||
                   (board[3] == playerChar && board[4] == playerChar && board[5] == playerChar) ||
                   (board[6] == playerChar && board[7] == playerChar && board[8] == playerChar) ||
                   (board[0] == playerChar && board[3] == playerChar && board[6] == playerChar) ||
                   (board[1] == playerChar && board[4] == playerChar && board[7] == playerChar) ||
                   (board[2] == playerChar && board[5] == playerChar && board[8] == playerChar) ||
                   (board[0] == playerChar && board[4] == playerChar && board[8] == playerChar) ||
                   (board[2] == playerChar && board[4] == playerChar && board[6] == playerChar);
        }

        public bool CheckDraw()
        {
            return FindLegalMovesIndices().Count == 0;
        }

        public Status CheckGameStatus()
        {
            if (CheckWin("X"))
            {
                return Status.XWIN;
            }
            if (CheckWin("O"))
            {
                return Status.OWIN;
            }
            if (CheckDraw())
            {
                return Status.DRAW;
            }
            return Status.ONGOING;
        }
    }
}
