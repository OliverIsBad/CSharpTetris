namespace CSharpTetris;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new GameForm());
        Console.WriteLine("Hello World");
    }    
}