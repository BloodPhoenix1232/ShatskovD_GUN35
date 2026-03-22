using GameECS;
using UnityEngine;
using System.Collections.Generic;

namespace Game.GameEngine.Ecs
{
    public sealed class VisionDetectionSystem : IEcsFixedUpdate
    {
        private readonly EcsWorld world;
        private readonly EcsPool<VisionComponent> visionPool;
        private readonly EcsPool<TransformComponent> transformPool;
        private readonly EcsPool<HitPointsComponent> hitPointsPool;
        private readonly EcsPool<AttackTarget> attackTargetPool;
        private readonly EcsPool<MoveToPositionData> moveToPositionPool;
        private readonly EcsPool<HitRequest> hitRequestPool;
        private readonly EcsPool<CombatComponent> combatPool;
        private readonly EcsPool<TeamComponent> teamPool;

        void IEcsFixedUpdate.FixedUpdate(int entity)
        {
            if (!this.visionPool.HasComponent(entity))
                return;

            ref var vision = ref this.visionPool.GetComponent(entity);
            ref var myTransform = ref this.transformPool.GetComponent(entity);
            var myPosition = myTransform.value.position;
            var radius = vision.radius;

            int nearestEnemy = -1;
            float nearestDistance = radius;

            int myTeam = this.teamPool.HasComponent(entity) ? this.teamPool.GetComponent(entity).teamId : -1;

            foreach (var enemy in this.world.GetEntities())
            {
                if (enemy == entity)
                    continue;

                if (!this.hitPointsPool.HasComponent(enemy))
                    continue;

                ref var enemyHp = ref this.hitPointsPool.GetComponent(enemy);
                if (enemyHp.current <= 0)
                    continue;

                if (this.teamPool.HasComponent(enemy))
                {
                    int enemyTeam = this.teamPool.GetComponent(enemy).teamId;
                    if (enemyTeam == myTeam)
                        continue;
                }

                ref var enemyTransform = ref this.transformPool.GetComponent(enemy);
                float distance = Vector3.Distance(myPosition, enemyTransform.value.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != -1)
            {
                if (this.attackTargetPool.HasComponent(entity))
                {
                    ref var currentTarget = ref this.attackTargetPool.GetComponent(entity);
                    if (currentTarget.targetId != nearestEnemy)
                    {
                        currentTarget.targetId = nearestEnemy;
                        this.UpdateMovement(entity, nearestEnemy);
                    }
                }
                else
                {
                    this.attackTargetPool.SetComponent(entity, new AttackTarget
                    {
                        targetId = nearestEnemy
                    });
                    this.UpdateMovement(entity, nearestEnemy);
                }
            }
            else
            {
                if (this.attackTargetPool.HasComponent(entity))
                {
                    this.attackTargetPool.RemoveComponent(entity);
                    this.moveToPositionPool.RemoveComponent(entity);
                    this.hitRequestPool.RemoveComponent(entity);
                }
            }
        }

        private void UpdateMovement(int entity, int targetId)
        {
            if (!this.transformPool.HasComponent(targetId))
                return;

            ref var targetTransform = ref this.transformPool.GetComponent(targetId);
            ref var combat = ref this.combatPool.GetComponent(entity);

            this.moveToPositionPool.SetComponent(entity, new MoveToPositionData
            {
                destination = targetTransform.value.position,
                stoppingDistance = combat.minDistance
            });
        }
    }
}