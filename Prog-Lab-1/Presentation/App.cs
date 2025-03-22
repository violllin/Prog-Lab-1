namespace Prog_Lab_1.Presentation;

public class App(
    IApplicationLifecycle applicationLifecycle,
    ICommandsPerformer commandsPerformer
) : IApplication
{
    public void Run()
    {
        Console.WriteLine(StringResource.Commands.Help);
        while (applicationLifecycle.IsRunning)
        {
            commandsPerformer.PerformCommands();
        }
        Console.WriteLine("Выключение.");
    }

    public IApplicationLifecycle ApplicationLifecycle => applicationLifecycle;
}