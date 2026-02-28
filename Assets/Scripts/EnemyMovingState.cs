using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovingState : BaseState
{
    private readonly EnemyStateMachine _stateMachine;

    public EnemyMovingState(EnemyStateMachine stateMachine)) : base("Moving", stateMachine)
    {
            _stateMachine = StateMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        _stateMachine.AnimationStateContoller.SetAnimation(CharacterAnimation.Walk);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _stateMachine.MovementController.UpdateMovement(_stateMachine.target.transform.position);

        if (Vector3.Distance(_stateMachine.transform.position, _stateMachine.target.transform.position) <= _stateMachine.hitDistance)
            _stateMachine.ChangeState(_stateMachine.EnemyAttackState);
    }

}
