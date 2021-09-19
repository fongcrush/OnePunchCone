using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;

public class PlayerAttackSkill03 : IPlayerAction
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
        attackInfo = PlayerAttackData.AttackTable[103];
    }


    public override void Begin()
    {
        Debug.Log("Attack Skill 03!");
        actionState = ActionState.Skill3;

        coll.gameObject.SetActive(true);
        player.stat.Power = Random.Range(attackInfo.min, attackInfo.max);
        attackInfo = PlayerAttackData.AttackTable[103];
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
                coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                coll.gameObject.SetActive(false);
                End();
            }
        }
        curTime += Time.deltaTime;
    }

	public override void End()
    {
        Debug.Log("Attack Skill 03 end!");
        actionState = ActionState.None;

        coll.gameObject.SetActive(false);
        isDone = true;
    }

	public override void Quit()
    {
        coll.gameObject.SetActive(false);
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
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
