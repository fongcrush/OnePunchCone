using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAttackData;

public abstract class PlayerAction : MonoBehaviour
{
    protected PlayerController player;

    protected PlayerActionMgr actionMgr;

    protected float curTime;

	protected bool isDone;

    public abstract bool Ready();

    public abstract void Quit();

    public abstract IEnumerator ActionRoutine();

    public IEnumerator SkillTimer(short code)
    {
        AttackTable[code].curTime = AttackTable[code].cTime;
        yield return null;

        while(AttackTable[code].curTime > 0f)
        {
            AttackTable[code].curTime -= Time.deltaTime;
            yield return null;
        }
        AttackTable[code].curTime = 0f;
        switch(code)
        {
        case 101:
        case 102:
            yield return GameObject.Find("UI").GetComponent<MainUIMgr>().SkillReady(code);
            break;
        }
    }
}