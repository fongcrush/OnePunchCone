using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class Skill3 : MonoBehaviour, IPlayerAttack
{
    private Player player;
    private PlayerAttackController attackController;
    private AttackInfo attackInfo;
    private Transform coll;
    private bool isDone;

    public Skill3(PlayerAttackController AttackController)
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        attackController = AttackController;
        attackInfo = PlayerAttackManager.attackTable[101];
        coll = GameObject.Find("AttackManager").transform.Find("Skill1Coll");
        isDone = false;
    }
    public void Awake()
    {
    }
    public void Run()
    {
        actionState = ActionState.Attack;
        player.stat.Power = Random.Range(attackInfo.min, attackInfo.max);
        isDone = false;
        StartCoroutine(AttackRoutine());
    }

    public void Quit()
    {
        isDone = true;
        attackController.End();
    }

    IEnumerator AttackRoutine()
    {
        attackState = AttackState.Skill3;

        StartCoroutine(PlayerAttackManager.SkillTimer(attackInfo.code));
        StartCoroutine(CheckDash());

        yield return new WaitForSeconds(attackInfo.delay);
        for(int i = 0; i < 4; i++)
        {
            coll.gameObject.SetActive(true);
            coll.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            yield return new WaitForSeconds(0.3f);
            coll.gameObject.SetActive(false);
            coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        yield return new WaitForSeconds(0.5f);
        Quit();
    }

    IEnumerator CheckDash()
    {
        while(!isDone)
        {
            if(actionState == ActionState.Dash)
            {
                StopCoroutine(AttackRoutine());
                isDone = false;
            }
            yield return null;
        }
    }
}
