using System;
using UnityEngine;

namespace Game.GameEngine.Ecs
{
    [Serializable]
    public struct DeathComponent
    {
        public bool isDead;
        public float deathTime;
    }
}