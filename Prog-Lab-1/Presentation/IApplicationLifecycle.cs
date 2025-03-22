namespace Prog_Lab_1.Presentation;

public interface IApplicationLifecycle
{
    public void Finish();
    public bool IsRunning { get; }
}