using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateDeneme : MonoBehaviour
{
    public abstract StateDeneme Runstate(Action action, AreaManager stat);
}
