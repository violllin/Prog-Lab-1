namespace Prog_Lab_1;

public static class ArrayType
{
    public enum Types
    {
        Int,
        Char,
        Varchar
    }
    
    public static bool ParseArrayType(string input, out Types result)
    {
        switch (input.ToLower())
        {
            case ArrayTypeTranscript.Int:
                result = Types.Int;
                return true;
            case ArrayTypeTranscript.Char:
                result = Types.Char;
                return true;
            case ArrayTypeTranscript.Varchar:
                result = Types.Varchar;
                return true;
            default:
                result = Types.Int;
                return false;
        }
    }

}