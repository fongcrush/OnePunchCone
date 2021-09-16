using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class Skill1 : MonoBehaviour, IPlayerAttack
{
    private Player player;
    private PlayerAttackController attackController;
    private AttackInfo attackInfo;
    private Transform coll;
    private bool isDone;

    public Skill1(PlayerAttackController AttackController)
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        attackController = AttackController;
        attackInfo = PlayerAttackManager.attackTable[101];
        coll      = GameObject.Find("AttackManager").transform.Find("Skill1Coll");
        isDone    = false;
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
        attackState = AttackState.Skill1;

        StartCoroutine(PlayerAttackManager.SkillTimer(attackInfo.code));
        StartCoroutine(CheckDash());

        coll.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackInfo.delay);
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        coll.gameObject.SetActive(false);
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(attackInfo.cTime - attackInfo.delay - 0.1f);
        Quit();
    }

    IEnumerator CheckDash()
    {
        while(isDone)
        {
            yield return null;
            isDone = false;
        }
    }
}