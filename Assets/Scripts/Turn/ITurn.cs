namespace Controllers
{
    public interface ITurn
    {
        Team Current { get; }
        void Next();
    }
}
