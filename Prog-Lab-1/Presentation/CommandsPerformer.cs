using Prog_Lab_1.Handlers;
using Prog_Lab_1.VM;

namespace Prog_Lab_1.Presentation;

public class CommandsPerformer(IVirtualMemoryService virtualMemoryService, IApplicationLifecycle applicationLifecycle): ICommandsPerformer
{
    public void PerformCommands()
    {
        var input = Console.ReadLine()?.Split(' ') ?? [];
        if (input.Length < 1)
        {
            PrintCommands();
            return;
        }

        var command = input[0].ToLower();
        var args = input.Skip(1).ToArray();
        switch (command)
        {
            case StringResource.Commands.Create.Command:
                new CreateCommandHandler(virtualMemoryService).HandleCommand(args);
                break;
            case StringResource.Commands.Input.Command:
                new InputCommandHandler(virtualMemoryService).HandleCommand(args);
                break;
            case StringResource.Commands.Print.Command:
                new PrintCommandHandler(virtualMemoryService).HandleCommand(args);
                break;
            case StringResource.Commands.Exit:
                new ExitCommandHandler(virtualMemoryService, applicationLifecycle).HandleCommand(args);
                break;
            default:
                PrintCommands();
                break;
        }
    }

    public void PrintCommands()
    {
        Console.WriteLine($"Команды:\n {StringResource.Commands.Help}");
    }
}