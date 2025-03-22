using Prog_Lab_1.Presentation;
using Prog_Lab_1.VM;

namespace Prog_Lab_1.Handlers;

public class ExitCommandHandler(IVirtualMemoryService virtualMemoryService, IApplicationLifecycle applicationLifecycle): ICommandHandler
{
    public void HandleCommand(string[] args)
    {
        try
        {
            virtualMemoryService.CloseVirtualMemory();
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("VirtualMemory не был запущен.");
        }
        applicationLifecycle.Finish();
    }
}