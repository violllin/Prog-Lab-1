namespace Prog_Lab_1;

public static class StringResource
{
    public static class Commands
    {
        public static class Create
        {
            public const string Command = "create";
            public const string Usage = "Использование: create <имя_файла> <тип_массива> <размер_массива> <длина строки - опционально>";
        }
        public static class Input
        {
            public const string Command = "input";
            public const string Usage = "Использование: input <индекс> <значение>";
        } 
        public static class Print
        {
            public const string Command = "print";
            public const string Usage = "Использование: print <индекс>";
        }
        
        public const string Exit = "exit";

        public const string Help = $"Введите команду ({Create.Command}, {Input.Command}, {Print.Command}, {Exit}):";
    }
}

public static class ArrayTypeTranscript
{
    public const string Int = "int";
    public const string Char = "char";
    public const string Varchar = "varchar";
}