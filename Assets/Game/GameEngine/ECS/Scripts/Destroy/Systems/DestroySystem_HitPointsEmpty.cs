using GameECS;
using System;

namespace Game.GameEngine.Ecs
{
    public sealed class DestroySystem_HitPointsEmpty : IEcsFixedUpdate
    {
        private readonly EcsPool<HitPointsComponent> hitPointsPool;
        private readonly EcsPool<AnimatorComponent> animatorPool;
        private readonly EcsEmitter<DestroyEvent> destroyEmitter;

        void IEcsFixedUpdate.FixedUpdate(int entity)
        {
            if (!this.hitPointsPool.HasComponent(entity))
            {
                return;
            }

            ref var hitPoints = ref this.hitPointsPool.GetComponent(entity);
            if (hitPoints.current <= 0 && !hitPoints.isDead)
            {
                hitPoints.isDead = true;

                if (this.animatorPool.HasComponent(entity))
                {
                    ref var animatorComp = ref this.animatorPool.GetComponent(entity);
                    animatorComp.value.PlayDeathAnimation("Death (5)", "Base Layer", () =>
                    {
                        this.destroyEmitter.SendEvent(entity, new DestroyEvent());
                    });
                }
                else
                {
                    this.destroyEmitter.SendEvent(entity, new DestroyEvent());
                }
            }
        }
    }
}