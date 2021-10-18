using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using static PlayerStatesData;
using static GameMgr;
using static PlayerEffectMgr;

public class PlayerAttackSkill01 : PlayerAction
{
    private AttackInfo attackInfo;

    private SkeletonAnimation skelAnim;

    private Animator anim;

    private BoxCollider2D boxColl;

    public void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        skelAnim = player.GetComponent<SkeletonAnimation>();
        anim = player.GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        actionMgr = player.ActionMgr;
        attackInfo = PlayerAttackData.AttackTable[101];
    }

	public override IEnumerator ActionRoutine()
    {
        StartCoroutine(SkillTimer(attackInfo.code));
        anim.SetTrigger("Skill1");
        actionState = ActionState.Skill1;
        isDone = false;

        yield return new WaitForSeconds(attackInfo.fDelay);
        PlayerEffect.PlayEffect(PlayerEffect.Skill01_Effect[0]);

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
            if (hitResults[i].collider.gameObject.tag == "EnemyHitColl")
            {
                Debug.Log(hitResults[i].collider.gameObject.name);
                int power = Random.Range(attackInfo.min, attackInfo.max);
                hitResults[i].collider.gameObject.transform.parent.GetComponent<EnemyController>().Hit(power);
            }
        }

        yield return new WaitForSeconds(attackInfo.sDelay);

        actionState = ActionState.None;
        isDone = true;
    }

	public override void Quit()
    {
        StopCoroutine(ActionRoutine());
        isDone = true;
    }

    public override bool Ready()
    {
        return PlayerAttackData.AttackTable[101].curTime == 0;
    }
}