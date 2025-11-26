using UnityEngine;
using Zenject;

namespace Controllers
{
	public class PlayerController : MonoBehaviour
	{
		private SignalBus _signal;      //Injected
        private ISharedData _data;      //Injected

		[Inject]
		private void Consturct(SignalBus signal, ISharedData data)
		{
			(_signal, _data) = (signal, data);
			_signal.Subscribe<GameEvent>(StartPlay);
		}

		private void StartPlay(GameEvent arg)
		{
			if (arg is not GameEvent.Confirm) return;
			if (_data.Status is not GameStatus.Confirm) return;

			_data.Status = GameStatus.Lock;
			var destination = _data.Destination;

			//Перемещение в клетку
            if (_data.Targer.IsEmpty)
            {
				destination.OnMoveEndCallBack += OnEndPlay;
				destination.Move(_data.Target);
            }
			//Атака
			else 
			{
				var target = _data.Target.Unit;
				target.Health -= destination.Setting.Stats.Damage;
				if (target.Health <= 0)
				{
					_data.Target.Unit == null;
					Destroy(target.GameObject);
				}

				_data.Target = null;
				_data.Status = GameStatus.Unlock;
			}
        }

		private void OnEndPlay()
		{
			_data.Status = GameStatus.Unlock;
			_data.Destination.OnMoveEndCallBack -= OnEndPlay;
			if (_data.Destination.Settings.Mobility.MoveAndAttackInTurn)
			{
				_data.Target = null;
				_data.Status = GameStatus.Attack;
			}
		}
    }
}