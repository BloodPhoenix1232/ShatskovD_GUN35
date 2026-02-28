using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : MonoBehaviour
{
    private readonly EnemyStateMachine _stateMachine;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base("Idle", stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        _stateMachine.AnimationStateController.SetAnimation(CharacterAnimation.Idle);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        TryDetectPlayer();
    }

    private void TryDetectPlayer()
    {
        foreach (var player in PlayersHolder.Instance.players)
        {
            if(Vector3.Distance(player.transform.possition, _stateMachine.transform.position) <= _stateMachine.detectionDistance)
            {
                _stateMachine.target = player;
                _stateMachine.ChangeState(_stateMachine.EnemyMovingState);
                break;
            }
        }
    }
}
