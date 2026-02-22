using Netologia.Behaviours;
using Netologia.TowerDefence;
using Netologia.TowerDefence.Behaviors;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Netologia.Systems
{
	public class ProjectileSystem : GameObjectPoolContainer<Projectile>, Director.IManualUpdate
	{
		private EffectSystem _effects;		//injected
		
		[SerializeField, Min(0.01f)]
		private float _hitDistance = 0.3f;
		
		public void ManualUpdate()
		{
			var deltaTime = TimeManager.DeltaTime;
			foreach (var projectiles in this)
			{
				foreach (var projectile in projectiles)
				{
					var transform = projectile.transform;
					var position = transform.position;
					var target = projectile.TargetPosition;

					var direction = Vector3.Normalize(target - position);
					position += direction * (projectile.MoveSpeed * deltaTime);
					transform.up = direction;
					transform.position = position;

					if(Vector3.SqrMagnitude(position - target) <= _hitDistance)
					{
						if(projectile.HasEffect)
						{
							var effect = _effects[projectile.HitEffect].Get;
							effect.transform.position = position;
							effect.Play();
						}

						projectile.DealDamage();
						this[projectile.Ref].ReturnElement(projectile.ID);
					}
				}
			}
		}

		public void OnDespawnUnit(int unitID)
		{
			foreach (var pool in this)
				foreach (var projectile in pool)
					if(projectile.TargetID == unitID)
						projectile.ResetTarget();
		}

		[Inject]
		private void Construct(EffectSystem effects)
		{
			(_effects) = (effects);
			//SqrtMagnitude optimization
			_hitDistance *= _hitDistance;
		}
	}
}