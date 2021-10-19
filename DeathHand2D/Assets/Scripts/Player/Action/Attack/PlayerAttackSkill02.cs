using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using static PlayerStatesData;
using static GameMgr;
using static PlayerEffectMgr;

public class PlayerAttackSkill02 : PlayerAction
{
    private Rigidbody2D rigid;

    private SkeletonAnimation skelAnim;

    private Animator anim;

    private BoxCollider2D boxColl;

    private AttackInfo attackInfo;
    public AttackInfo Info { get { return attackInfo; } }

    private List<GameObject> effectList;

    private List<ParticleSystem> particleSyetemList = new List<ParticleSystem>();

    private Vector3 startPos, dirPos;

    private bool canCombo = false;
    public bool CanCombo { get { return canCombo; } set { canCombo = value; } }

    private bool comboOn = false;
    public bool ComboOn { get { return comboOn; } }

    private bool on = false;

	public void Awake()
    {
        player = GM.Player.GetComponent<PlayerController>();
        skelAnim = player.GetComponent<SkeletonAnimation>();
        anim = player.GetComponent<Animator>();
        rigid = player.GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
        effectList = PlayerEffect.Skill02_Effect;
        foreach(var effect in effectList)
            particleSyetemList.Add(effect.GetComponent<ParticleSystem>());
        dirPos = Vector3.zero;
        startPos = Vector3.zero; ;
    }

    private void Start()
    {
        actionMgr = player.ActionMgr;
        attackInfo = PlayerAttackData.AttackTable[102];
    }

    public override IEnumerator ActionRoutine()
    {
        StartCoroutine(SkillTimer(attackInfo.code));
        anim.SetTrigger("Skill2");
        actionState = ActionState.Skill2;
        isDone = true;

        startPos = player.transform.position;

        // 돌진 목표 지점 계산
        if(player.LeftOrRight())
            dirPos = new Vector3(player.transform.position.x - 5f, player.transform.position.y, 0);
        else
            dirPos = new Vector3(player.transform.position.x + 5f, player.transform.position.y, 0);
        dirPos.x = Mathf.Clamp(dirPos.x, GM.CurRoomMgr.MapSizeMin.x, GM.CurRoomMgr.MapSizeMax.x);

        yield return new WaitForSeconds(attackInfo.fDelay);

        Vector3 effectPostion = player.transform.position;
        if(player.LeftOrRight())
            effectPostion.x += 0.5f;
        else
            effectPostion.x -= 0.5f;
        Quaternion effectQuaternion = Quaternion.Euler(0, player.transform.eulerAngles.y, 0f);

        Destroy(
            Instantiate(
            effectList[0],
            effectPostion,
            effectQuaternion
            ), particleSyetemList[0].main.duration
        );

        effectPostion = (startPos + player.transform.position) / 2;
        effectQuaternion = Quaternion.Euler(-90f, 0f, 0f);

        Destroy(
            Instantiate(
            effectList[1],
            effectPostion,
            effectQuaternion
            ), particleSyetemList[1].main.duration
        );

        Destroy(
            Instantiate(
            effectList[2],
            effectPostion,
            effectQuaternion
            ), particleSyetemList[2].main.duration
        );

        Vector2 castDir;
        if(player.LeftOrRight())
            castDir = Vector2.left;
        else
            castDir = Vector2.right;

        on = true;
        curTime = 0;
        RaycastHit2D[] hitResults = new RaycastHit2D[100];
        while(curTime  < 1f)
        {
            for(int i =0; i< boxColl.Cast(transform.position, hitResults, 0); i++)
            {
                if (hitResults[i].collider.gameObject.tag == "EnemyHitColl")
                {
                    var obj = hitResults[i].collider.transform.parent;
                    obj.transform.position = new Vector3(player.transform.position.x + castDir.x, obj.transform.position.y, 0);
                }
            }
            rigid.MovePosition(Vector3.Lerp(rigid.position, dirPos, Time.deltaTime * 20));
            curTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        on = false;
        actionState = ActionState.None;
        isDone = true;

        yield return CheckCombo(5f);
    }

    private IEnumerator CheckCombo(float checkTime)
	{
        canCombo = true;
        float curTime = 0f;
        while(checkTime > curTime)
		{
            yield return null;
            curTime += Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.C))
			{
                actionMgr.Skill_03_On = true;
                break;
            }
        }
        canCombo = false;
    }

    public override void Quit()
    {
        StopCoroutine(ActionRoutine());
        StartCoroutine(CheckCombo(5f));
        isDone = true;
    }

    public override bool Ready()
    {
        return PlayerAttackData.AttackTable[102].curTime == 0;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if(collision.gameObject.tag == "EnemyHitColl" && on)
        {
            int power = Random.Range(attackInfo.min, attackInfo.max);
            collision.transform.parent.GetComponent<EnemyController>().Hit(power);
        }
    }
}
