using GameECS;
using UnityEngine;

namespace Game.GameEngine.Ecs
{
    [CreateAssetMenu(
        fileName = "New Installer «Move»",
        menuName = "Game/GameEngine/Ecs/New Installer «Move»"
    )]
    public sealed class MoveInstaller : EcsInstaller
    {
        public override void Install(EcsWorld world)
        {
            world.DeclareComponent<MoveSpeedComponent>();
            world.DeclareComponent<MoveStateComponent>();
            world.DeclareComponent<VisionComponent>();

            world.DeclareComponent<MoveStepData>();
            world.DeclareComponent<MoveToPositionData>();
            world.DeclareComponent<PatrolData>();
            world.DeclareComponent<DeathComponent>();
            world.DeclareComponent<DeathAnimationCompleteEvent>();

            world.DeclareSystem<MoveStepSystem>();
            world.DeclareSystem<MoveToPositionSystem>();
            world.DeclareSystem<PatrolPointsSystem>();
            world.DeclareSystem<VisionDetectionSystem>();
        }
    }
}