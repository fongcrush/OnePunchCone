using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using static PlayerStatesData;
using static GameMgr;

public class PlayerAttackSkill03 : PlayerAction
{
    private AttackInfo attackInfo;

    private SkeletonAnimation skelAnim;

    private Animator anim;

    private BoxCollider2D boxColl;

    private int count;

    public void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        skelAnim = player.GetComponent<SkeletonAnimation>();
        anim = player.GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();

        count = 0;
    }

	public void Start()
	{
        actionMgr = player.ActionMgr;
        attackInfo = PlayerAttackData.AttackTable[103];
    }

    public override IEnumerator ActionRoutine()
    {
        StartCoroutine(SkillTimer(attackInfo.code));
        actionState = ActionState.Skill3;
        count = 0;
        curTime = 0;
        isDone = false;


        yield return new WaitForSeconds(attackInfo.fDelay);
        while(count < 4)
        {
            anim.SetTrigger("Skill3");

            yield return new WaitForSeconds(0.1f);

            RaycastHit2D[] hitResults = new RaycastHit2D[100];
            for(int i = 0; i < boxColl.Cast(Vector2.left, hitResults, 0); i++)
            {
                if (hitResults[i].collider.gameObject.tag == "Jangseung")
                {
                    Debug.Log(hitResults[i].collider.gameObject.name);
                    int power = Random.Range(attackInfo.min, attackInfo.max);
                    hitResults[i].collider.gameObject.transform.GetComponent<Jangseung>().Hit(power);
                    break;
                }
                if (hitResults[i].collider.gameObject.tag == "PowderKeg")
                {
                    Debug.Log(hitResults[i].collider.gameObject.name);
                    int power = Random.Range(attackInfo.min, attackInfo.max);
                    hitResults[i].collider.gameObject.transform.GetComponent<PowderKeg>().Hit(power);
                    break;
                }
                if (hitResults[i].collider.gameObject.tag == "Enemy")
                {
                    Debug.Log(hitResults[i].collider.gameObject.name);
                    int power = Random.Range(attackInfo.min, attackInfo.max);
                    hitResults[i].collider.gameObject.transform.parent.GetComponent<EnemyController>().Hit(power);
                }
            }

            yield return new WaitForSeconds(0.1f);
            count++;
        }

        yield return new WaitForSeconds(attackInfo.sDelay);

        //Debug.Log("Attack Skill 03 end!");
        actionState = ActionState.None;
        actionMgr.Skill_03_On = false;
        isDone = true;
    }

	public override void Quit()
    {
        StopCoroutine(ActionRoutine());
        actionMgr.Skill_03_On = false;
        isDone = true;
    }

    public override bool Ready()
    {
        return true;
    }
}
