using Prog_Lab_1.VM;

namespace Prog_Lab_1.Handlers;

public class InputCommandHandler(IVirtualMemoryService service): ICommandHandler
{
    public void HandleCommand(string[] args)
    {
        if (!service.IsActive)
        {
            Console.WriteLine("Для начала создайте массив." + StringResource.Commands.Create.Usage);
            return;
        }
        
        if (args.Length < 2)
        {
            Console.WriteLine(StringResource.Commands.Input.Usage);
            return;
        }

        if (!int.TryParse(args[0], out int index))
        {
            Console.WriteLine("Неправильный индекс. +" + StringResource.Commands.Input.Usage);
            return;
        }
        var arrayType = service.ActiveArrayType;
        InputToArray(index, args.Skip(1).ToArray(), arrayType);
    }

    private void InputToArray(int index, string[] args, ArrayType.Types? arrayType)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Укажите значение для записи. " + StringResource.Commands.Input.Usage);
            return;
        }
        var value = args[0];

        switch (arrayType)
        {
            case ArrayType.Types.Int:
                if (!int.TryParse(value, out int intValue))
                {
                    Console.WriteLine("Неправильное значение для массива int. " + StringResource.Commands.Input.Usage);
                    return;
                }
                service.InsertValueToActiveVirtualMemory(index, intValue);
                break;
            case ArrayType.Types.Char:
            case ArrayType.Types.Varchar:
                service.InsertValueToActiveVirtualMemory(index, value);
                break;
            case null:
                Console.WriteLine("Для начала создайте массив." + StringResource.Commands.Create.Usage);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Console.WriteLine("Успешно!");
    }
}