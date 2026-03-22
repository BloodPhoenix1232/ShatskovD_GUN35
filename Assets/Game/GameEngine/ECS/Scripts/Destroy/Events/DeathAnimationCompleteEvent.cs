using System;

namespace Game.GameEngine.Ecs
{
    [Serializable]
    public struct DeathAnimationCompleteEvent
    {
        public int entity;
    }
}