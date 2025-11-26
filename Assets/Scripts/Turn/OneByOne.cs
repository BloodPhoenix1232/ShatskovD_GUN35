using System.Collections.Generic;

namespace Controllers
{
    public class OneByOne : ITurn
    {
        private int _index;
        private readonly IReadOnlyList<Team> _teams;

        public Team Current => _teams[index];

        public void Next()
        {
            _index = (_index + 1) % _teams.Count;
        }

        private OneByOne(IReadOnlyList<Team> teams)
        {
            _teams = teams;
            _index = UnityEngine.Random.Range(0, _teams.Count);
        }
    }
}