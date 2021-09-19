using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;

public class PlayerAttackSkill01 : IPlayerAction
{
    [SerializeField]
    private Transform coll;

    private AttackInfo attackInfo;

	public void Awake()
	{
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        actionMgr = player.ActionMgr;

        curTime = 0;
        isDone = false;
    }

    private void Start()
    {
        attackInfo = PlayerAttackData.AttackTable[101];
    }

    public override void Begin()
    {
        Debug.Log("Attack Skill 01!");
        actionState = ActionState.Skill1;

        coll.gameObject.SetActive(true);
        player.stat.Power = Random.Range(attackInfo.min, attackInfo.max);
        attackInfo = PlayerAttackData.AttackTable[101];
        curTime = 0;
        isDone = false;

        StartCoroutine(PlayerAttackData.SkillTimer(attackInfo.code));
        StartCoroutine(CheckDash());
    }

	public override void UpdateAction()
    {
        if(curTime < attackInfo.fDelay) { } // �� ������
        else if(curTime < attackInfo.sDelay + 0.1f)
        {
            if(coll.GetComponent<BoxCollider2D>().enabled == false)
                coll.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if(curTime < attackInfo.sDelay) { } // �� ������
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
        Debug.Log("Attack Skill 01 end!");
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