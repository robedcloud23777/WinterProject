using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType { None, Default, Global }

public class StateMachine<T> where T : class
{
    private T origin;
    Dictionary<string, State<T>> states;
    private string currentState;
    private string globalState;

    public void Setup(T ownerOfStateMachine)
    {
        states = new Dictionary<string, State<T>>();

        origin = ownerOfStateMachine;
        currentState = null;
        globalState = null;
    }

    public void Execute()
    {
        if (globalState != null) states[globalState].Execute(origin);
        if (currentState != null) states[currentState].Execute(origin);
    }

    public void Tick()
    {
        if (globalState != null) states[globalState].Tick(origin);
        if (currentState != null) states[currentState].Tick(origin);
    }

    public void AddState(string str, State<T> newState, StateType type)
    {
        states[str] = newState;
        if(type == StateType.Default)
            ChangeState(str);

        else if(type == StateType.Global)
            SetGlobalState(str);
    }

    public void ChangeState(string newState)
    {
        if (currentState == newState) return;

        if (currentState != null)
            states[currentState].Exit(origin);

        currentState = newState;
        states[currentState].Enter(origin);
    }

    public void SetGlobalState(string str) 
        => globalState = str;

}




