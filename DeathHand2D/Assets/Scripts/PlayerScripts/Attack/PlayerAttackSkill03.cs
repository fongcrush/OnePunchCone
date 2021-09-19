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

        curTime = 0;
        isDone = false;
    }

    private void Start()
    {
        actionMgr = player.ActionMgr;
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
    }

    public override void UpdateAction()
	{
        if(curTime < attackInfo.fDelay) { } // �� ������
        else if(curTime < attackInfo.fDelay + 0.1f)
        {
            if(coll.GetComponent<BoxCollider2D>().enabled == false)
                coll.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if(curTime < attackInfo.sDelay) { } // �� ������
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
        actionState = ActionState.Dash;
        actionMgr.End();
    }
}
