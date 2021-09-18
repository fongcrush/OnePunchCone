using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

[RequireComponent(typeof(PlayerAttackController))]
public class AAttack : MonoBehaviour, IPlayerAttack
{
    private Player player;
    private PlayerAttackController attackController;
    private AttackInfo attackInfo;
    private Transform coll;
    private bool isDone;

    private void Awake()
    {
        isDone = false;
    }

    private void Start()
    {
        attackController = GetComponent<PlayerAttackController>();
        if(attackController.gameObject.name == "@GM") { attackController = null; }

        player = GameObject.Find("Player").GetComponent<Player>();
        coll = GameObject.Find("AttackManager").transform.Find("Skill1Coll");
        attackInfo = PlayerAttackManager.attackTable[100];
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
        attackState = AttackState.AAttack;

        StartCoroutine(PlayerAttackManager.SkillTimer(attackInfo.code));
        StartCoroutine(CheckDash());

        coll.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackInfo.delay);
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        coll.gameObject.SetActive(false);
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Quit();
        yield return new WaitForSeconds(attackInfo.cTime/1000.0f - attackInfo.delay - 0.1f);
    }

    IEnumerator CheckDash()
	{
        while(!isDone)
		{
            if(actionState == ActionState.Dash)
            {
                StopCoroutine(AttackRoutine());
            }
            yield return null;
        }
    }
}