namespace CSharpTetris;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        //Application.Run(new GameForm());
        Application.Run(new GameOverForm());
        Console.WriteLine("This is tetris");
    }    
}