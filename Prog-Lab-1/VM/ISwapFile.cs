namespace VM.Domain;

public interface ISwapFile : IDisposable
{
    public IPage LoadPage(int pageIndex);
    
    public void SavePage(IPage page);
}