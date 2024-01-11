using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine: MonoBehaviour
{
    [SerializeField] protected State _currentState;

    public void ChangeState(State state)
    {
        if (_currentState != null)
            _currentState.Leave();
        state.Enter();
        _currentState = state;
    }

    public State CurrentState { get { return _currentState; } }
}
