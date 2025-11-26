using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class HeroesCommand : IGameplayCommand
    {
        public IEnumerable<Cell> Variants => throw new System.NotImplementedException();

        public void Interact(Cell cell)
        {
            throw new System.NotImplementedException();
        }
    }
}
