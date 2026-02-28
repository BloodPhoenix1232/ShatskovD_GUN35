using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyStateMachine : StateMachine
{
    public readonly float hitDistance = 0.9f;
    public readonly float detectionDistance = 6f;
    public readonly Vector2 hitDelay = new Vector2(1, 2);

    [HideInInspector] public EnemyIdleState EnemyIdleState;
    [HideInInspector] public EnemyMovingState EnemyMovingState;
    [HideInInspector] public EnemyHitState EnemyHitState;
    [HideInInspector] public EnemyAttackState EnemyAttackState;
    [HideInInspector] public EnemyHittedState EnemyHittedState;

    public EnemyMovementController MovementController { get; private set; }

    public EnemyStateMachine target;

    private void Awake()
    {
        MovementController = GetComponent<EnemyMovementContoller>();
        AnimationStateContoller = GetComponent<AnimationStateContoller>();

        EnemyIdleState = new EnemyIdleState(this);
        EnemyMovingState = new EnemyMovingState(this);
        EnemyHitState = new EnemyHitState(this);
        EnemyAttackState = new EnemyAttackState(this);
        EnemyHittedState = new EnemyHittedState(this);
    }
    
    public void Die()
    {
        Destroy(GameObject);
    }

    protected override BaseState GetInitialState()
    {
        return EnemyIdleState;
    }
}
