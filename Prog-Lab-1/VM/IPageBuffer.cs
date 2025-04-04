namespace VM.Domain;

public interface IPageBuffer : IDisposable
{
    public IPage GetPage(int pageIndex);

    public void MarkPageModified(int pageIndex);
}