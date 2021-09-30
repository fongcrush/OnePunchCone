using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAttackData;

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
    public abstract bool Ready();

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