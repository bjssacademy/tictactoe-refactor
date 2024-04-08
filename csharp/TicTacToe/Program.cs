namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TicTacToe.Game.Game game = new TicTacToe.Game.Game();    
            game.Setup();
            game.Start();
        }
    }
}
