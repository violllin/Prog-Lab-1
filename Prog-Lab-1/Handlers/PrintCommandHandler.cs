using Prog_Lab_1.VM;

namespace Prog_Lab_1.Handlers;

public class PrintCommandHandler(IVirtualMemoryService service): ICommandHandler
{
    public void HandleCommand(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine(StringResource.Commands.Print.Usage);
            return;
        }

        if (!int.TryParse(args[0], out int index))
        {
            Console.WriteLine("Неправильный индекс. " + StringResource.Commands.Print.Usage);
            return;
        }
        Console.WriteLine(service.GetItemByIndex(index));
    }
}