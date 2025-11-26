using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class BattleController : MonoBehaviour
    {
        private IGamePlayCommand _command;          //Injected
        private ISharedData _data;                  //Injected
        private SignalBus _signal;                  //Injected
        private Controls.MainActions _controls;     //Injected
        private Battlefield _battlefield;           //Injected

        private void OnCancel(Input.Action.CallBackContext obj)
        {
            _data.Event = GameEvent.Cancel;
            _data.Status = GameStatus.Select;
        }

        private void OnConfirm(Input.Action.CallBackContext obj)
        {
            if (_data.Destination == null)
            {
                Debug.Log("Non selected cell");
                return;
            }

            _signal.Fire(GameStatus.Confirm);
            _signal.Fire(GameEvent.Confirm);
        }

        private void CallBack(GameEvent arg)
        {
            if (arg is not GameEvent.Select) return;
            switch (_data.Status)
            {
                case GameStatus.Select:
                    _signal.Fire(GameStatus.Move);
                    break;
                case GameStatus.Move:
                    _signal.Fire(_data.Destination.Settings.Mobility.MoveAndAttackInTurn
                        ? GameStatus.Attack
                        : GameStatus.Confirm);
                    break;
                case GameStatus.Attack:
                    _signal.Fire(GameStatus.Confirm);
                    break;
                case GameStatus.Confirm:
                    Debug.LorError("Incorrect value");
                    break;
            }
        }

        [Inject]
        private void Construct(IGameplayCommand command, ISharedData data, SignalBus signal, BattleField battlefield, Controls.MainActions controls)
        {
            (_command, _data, _signal, _battlefield, _controls) = (command, data, signal, battlefield, controls);

            _battlefield.OnCellClicked += _command.Interact;
            _controls.Cancel.performed += OnCancel;
            _controls.Confirm.performed += OnConfirm;
            _signal.Subrscribe<GameEvent>(CallBack);
        }

        private void OnDestroy()
        {
            _battlefield.OnCellClicked -= _command.Interact;
            _controls.Cancel.performed -= OnCancel;
            _controls.Confirm.performed -= OnConfirm;
        }
    }
}