using Game.GameEngine.Ecs;
using SampleProject;
using UnityEngine;

namespace Entities
{
    public sealed class CharacterEntity : Entity
    {
        [SerializeField]
        private CharacterConfig config;

        protected override void Init()
        {
            this.SetData(new SmoothRotationComponent());

            this.SetData(new CombatComponent
            {
                damage = this.config.damage,
                minDistance = config.minDistance,
                animationTime = this.config.animationTime,
                timeBetweenAttack = this.config.timeBetweenAttack,
                damageType = this.config.damageType
            });
            
            this.SetData(new AnimatorComponent
            {
                value = this.GetComponentInChildren<AnimatorMachine>()
            });

            this.SetData(new HitPointsComponent
            {
                max = this.config.hitPoints,
                current = this.config.hitPoints,
                isDead = false
            });

            this.SetData(new MoveSpeedComponent
            {
                value = this.config.moveSpeed
            });

            this.SetData(new TransformComponent
            {
                value = this.transform,
                radius = this.config.radius
            });

            this.SetData(new GameObjectComponent
            {
                value = this.gameObject
            });

            this.SetData(new RigidbodyComponent
            {
                value = this.GetComponent<Rigidbody>()
            });

            this.SetData(new RendererComponent
            {
                value = this.GetComponentInChildren<Renderer>()
            });

            this.SetData(new MoveStateComponent
            {
                moveRequired = false,
                direction = Vector3.zero
            });

            this.SetData(new DeathComponent
            {
                isDead = false,
                deathTime = 0
            });

            this.SetData(new VisionComponent
            {
                radius = this.config.visionRadius = 10f
            });

            this.SetData(new TeamComponent
            {
                teamId = this.config.teamId
            });
        }
    }
}