using Prog_Lab_1.VM;

namespace Prog_Lab_1.Handlers;

public class CreateCommandHandler(IVirtualMemoryService service) : ICommandHandler
{
    public void HandleCommand(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine(StringResource.Commands.Create.Usage);
            return;
        }
        var fileName = args[0];
        if (!ArrayType.ParseArrayType(args[1], out var arrayType))
        {
            Console.WriteLine("Неправильный тип массива. " + StringResource.Commands.Create.Usage);
            return;
        }

        if (!int.TryParse(args[2], out var size))
        {
            Console.WriteLine("Неправильный размер массива. " + StringResource.Commands.Create.Usage);
            return;
        }

        var remainingLines = args.Skip(3).ToArray();
        HandleArrayType(fileName, arrayType, size, remainingLines);
    }

    private void HandleArrayType(string fileName, ArrayType.Types type, int size, string[] remainingLines)
    {
        switch (type)
        {
            case ArrayType.Types.Int:
                service.CreateIntVirtualMemory(fileName, size);
                break;
            case ArrayType.Types.Char:
                if (remainingLines.Length < 1 || !int.TryParse(remainingLines[0], out var stringLength))
                {
                    Console.WriteLine("Неправильная длина строки. " + StringResource.Commands.Create.Usage);
                    return;
                }
                service.CreateCharVirtualMemory(fileName, size, stringLength);
                break;
            case ArrayType.Types.Varchar:
                if (remainingLines.Length < 1 || !int.TryParse(remainingLines[0], out var maxStringLength))
                {
                    Console.WriteLine("Неправильная длина строки. " + StringResource.Commands.Create.Usage);
                    return;
                }
                service.CreateVarcharVirtualMemory(fileName, size, maxStringLength);
                break;
            default:
                throw new ArgumentException("Array type is not supported.");
        }
        Console.WriteLine("Успешно!");
    }
}