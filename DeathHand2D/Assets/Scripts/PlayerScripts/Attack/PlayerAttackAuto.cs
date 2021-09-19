using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;

public class PlayerAttackAuto : IPlayerAction
{
    [SerializeField]
    private Transform coll;

    private AttackInfo attackInfo;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        actionMgr = player.ActionMgr;

        curTime = 0;
        isDone = false;
    }

    private void Start()
	{
        attackInfo = PlayerAttackData.AttackTable[100];
    }

    public override void Begin()
    {
        Debug.Log("Auto Attack!");
        actionState = ActionState.AAttack;

        coll.gameObject.SetActive(true);
        player.stat.Power = Random.Range(attackInfo.min, attackInfo.max);
        attackInfo = PlayerAttackData.AttackTable[100];
        curTime = 0;
        isDone = false;

        StartCoroutine(PlayerAttackData.SkillTimer(attackInfo.code));
        StartCoroutine(CheckDash());
	}

	public override void UpdateAction()
    {
        if(curTime < attackInfo.fDelay) { } // ¼± µô·¹ÀÌ
        else if(curTime < attackInfo.fDelay + 0.1f)
        {
            if(coll.GetComponent<BoxCollider2D>().enabled == false)
                coll.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if(curTime < attackInfo.sDelay) { } // ÈÄ µô·¹ÀÌ
        else
        {
            if(coll.gameObject.activeSelf)
            {
                coll.GetComponent<BoxCollider2D>().enabled = false;
                End();
            }
        }
        curTime += Time.deltaTime; 
    }

	public override void End()
    {
        Debug.Log("Auto Attack end!");
        actionState = ActionState.None;

        coll.gameObject.SetActive(false);
        isDone = false;
	}

	public override void Quit()
    {
        coll.gameObject.SetActive(false);
        coll.GetComponent<BoxCollider2D>().enabled = false;
        coll.gameObject.SetActive(true);
        isDone = true;
        actionMgr.End();
    }

	IEnumerator CheckDash()
	{
        while(!isDone)
		{
            if(actionState == ActionState.Dash)
                Quit();
            yield return null;
        }
    }
}