using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;

public class PlayerAttackSkill03 : IPlayerAction
{
    [SerializeField]
    private Transform coll;

    private AttackInfo attackInfo;

    [SerializeField]
    private ActionStep attackStep;

    private int count;

    public void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        attackStep = ActionStep.None;
        count = 0;

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

        attackInfo = PlayerAttackData.AttackTable[103];
        attackStep = ActionStep.First_Delay;
        count = 0;
        curTime = 0;
        isDone = false;

        StartCoroutine(PlayerAttackData.SkillTimer(attackInfo.code));
    }

    public override void UpdateAction()
	{
        switch(attackStep)
		{
        case ActionStep.First_Delay:
            if(curTime < attackInfo.fDelay) { }
            else
            {
                curTime = 0;
                attackStep = ActionStep.Action;
            }
            break;
        case ActionStep.Action:
            if(count < 4)
            {
                if(curTime < 0.05f)
                {
                    if(!coll.transform.gameObject.activeSelf)
                    {
                        player.stat.Power = Random.Range(attackInfo.min, attackInfo.max);
                        coll.gameObject.SetActive(true);
                        coll.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
                else if(curTime < 0.1f) { }
                else if(curTime < 0.3f)
                {
                    if(coll.gameObject.activeSelf)
                    {
                        coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                        coll.gameObject.SetActive(false);
                    }
                }
                else
                {
                    ++count;
                    curTime = 0;
                }
            }
            else
            {
                count = 0;
                attackStep = ActionStep.Second_Delay;
            }
            break;
        case ActionStep.Second_Delay:
            if(curTime < attackInfo.sDelay) { }
            else
            {
                curTime = 0;
                End();
            }
            break;
        }
        curTime += Time.deltaTime;
    }

	public override void End()
    {
        Debug.Log("Attack Skill 03 end!");
        actionState = ActionState.None;
        actionMgr.skill_03_On = false;

        coll.gameObject.SetActive(false);
        isDone = true;
    }

	public override void Quit()
    {
        coll.gameObject.SetActive(false);
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        actionMgr.skill_03_On = false;

        isDone = true;
        actionMgr.End();
    }

    public override bool Ready()
    {
        return attackInfo.curTime == 0;
    }
}
