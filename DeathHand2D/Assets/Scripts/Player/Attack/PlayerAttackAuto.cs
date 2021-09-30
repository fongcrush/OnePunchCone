using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static GameMgr;

public class PlayerAttackAuto : IPlayerAction
{
    [SerializeField]
    private Transform coll;

	private AttackInfo attackInfo;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        curTime = 0;
        isDone = false;
    }

    private void Start()
	{
        actionMgr = player.ActionMgr;
        attackInfo = PlayerAttackData.AttackTable[100];
    }

    public override void Begin()
    {
        //Debug.Log("Auto Attack!");
        actionState = ActionState.AAttack;

        coll.gameObject.SetActive(true);
        GM.pcStat.Power = Random.Range(attackInfo.min, attackInfo.max);
        attackInfo = PlayerAttackData.AttackTable[100];
        curTime = 0;
        isDone = false;

        StartCoroutine(SkillTimer(attackInfo.code));
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
        //Debug.Log("Auto Attack end!");
        actionState = ActionState.None;

        coll.gameObject.SetActive(false);
        isDone = false;
	}

	public override void Quit()
    {
        coll.GetComponent<BoxCollider2D>().enabled = false;
        coll.gameObject.SetActive(false);
        isDone = true;
        actionMgr.End();
    }

	public override bool Ready()
    {
        return PlayerAttackData.AttackTable[100].curTime == 0;
    }
}