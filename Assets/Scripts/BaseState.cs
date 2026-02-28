using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour
{
    public string name;
    protected StateMachine stateMachine;

    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }

}
