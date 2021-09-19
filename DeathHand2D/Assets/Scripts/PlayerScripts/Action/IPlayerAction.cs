using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerAction : MonoBehaviour
{
    protected PlayerController player;

    protected PlayerActionMgr actionMgr;

    protected float curTime;

    protected bool isDone;

    public abstract void Begin();
    public abstract void UpdateAction();
    public abstract void End();
    public abstract void Quit();

    public IEnumerator Timer(float dirTime) 
    { 
        yield return null; 
    }
}