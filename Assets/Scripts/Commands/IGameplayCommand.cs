using System.Collections.Generic;

namespace Commands
{
    public interface IGameplayCommand
    {
        IEnumerable<Cell> Variants { get; }
        void Interact(Cell cell);
    }
}