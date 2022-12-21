using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_1 : MonoBehaviour
{
    public StateDeneme currentState;
    public AreaManager areaManager;
    public Action action;
    void Awake()
    {
        areaManager = GetComponent<AreaManager>();
        action = GetComponent<Action>();
    }

    // Update is called once per frame
    void Update()
    {
        RunstateMachine();
    }
    void RunstateMachine()
    {
        StateDeneme state = currentState.Runstate(action, areaManager);
        if (currentState != null)
        {
            currentState = state;

        }
    }
}
