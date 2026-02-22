using Netologia.Behaviours;
using Netologia.TowerDefence;
using Netologia.TowerDefence.Behaviors;
using Zenject;

namespace Netologia.Systems
{
	public class TowerSystem : GameObjectPoolContainer<Tower>, Director.IManualUpdate
	{
		private UnitSystem _units;				//injected
		private ProjectileSystem _projectiles;  //injected

		public void ManualUpdate()
		{
			float deltaTime = TimeManager.DeltaTime;

			foreach(var towers in this)
			{
				foreach(var tower in towers)
				{
                    if (!tower.DecrementAttackReload(deltaTime))
					{
						continue;
					}

					var position = tower.transform.position;

                    if (!tower.HasTarget)
					{
						var nearestTarget = _units.FindTarget(position, tower.Range);

						if(nearestTarget == null)
						{
							continue;
						}

						tower.Target = nearestTarget;
					}

                    var projectile = _projectiles[tower.Projectile].Get;
                    projectile.PrepareData(position, tower.Target, tower.Damage, tower.AttackElemental);

					tower.Attack();                   
				}
			}
		}

		public void OnDespawnUnit(int unitID)
		{
			foreach (var pair in this)
				foreach (var tower in pair)
					if (tower.TargetID == unitID)
						tower.Target = null;
		}
		
		[Inject]
		private void Construct(UnitSystem units, ProjectileSystem projectiles)
		{
			_units = units;
			_projectiles = projectiles;
		}
	}
}