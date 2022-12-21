using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deneme_state_2 : StateDeneme
{
    public State_1 state;
    public Deneme_state_2 stat2;
    public override StateDeneme Runstate(Action action, AreaManager stat)
    {
        if (true)
        {
            return stat2;

        }


    }
}