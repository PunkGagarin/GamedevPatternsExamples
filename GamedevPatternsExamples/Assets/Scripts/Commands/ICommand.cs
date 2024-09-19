namespace DefaultNamespace
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}