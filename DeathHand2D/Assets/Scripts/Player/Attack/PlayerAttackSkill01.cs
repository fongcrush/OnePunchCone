using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static GameMgr;

public class PlayerAttackSkill01 : IPlayerAction
{
    [SerializeField]
    private Transform coll;

    private AttackInfo attackInfo;

    public void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        curTime = 0;
        isDone = false;
    }

    private void Start()
    {
        actionMgr = player.ActionMgr;
        attackInfo = PlayerAttackData.AttackTable[101];
    }

	public override void Begin()
    {
        //Debug.Log("Attack Skill 01!");
        actionState = ActionState.Skill1;

        coll.gameObject.SetActive(true);
        GM.pcStat.Power = Random.Range(attackInfo.min, attackInfo.max);
        attackInfo = PlayerAttackData.AttackTable[101];
        curTime = 0;
        isDone = false;

        StartCoroutine(SkillTimer(attackInfo.code));
    }

	public override void UpdateAction()
    {
        
        if(curTime < attackInfo.fDelay) { }
        else if(curTime < attackInfo.sDelay + 0.1f)
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
                coll.gameObject.SetActive(false);
                End();
            }
        }
        curTime += Time.deltaTime;
    }

    public override void End()
    {
        actionState = ActionState.None;

        coll.gameObject.SetActive(false);
        isDone = true;
    }

	public override void Quit()
    {
        isDone = true;
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        coll.gameObject.SetActive(false);
        actionMgr.End();
    }

    public override bool Ready()
    {
        return PlayerAttackData.AttackTable[101].curTime == 0;
    }
}