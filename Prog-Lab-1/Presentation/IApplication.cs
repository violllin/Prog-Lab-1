namespace Prog_Lab_1.Presentation;

public interface IApplication
{
    public void Run();
    public IApplicationLifecycle ApplicationLifecycle { get; }
}