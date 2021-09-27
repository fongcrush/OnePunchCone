using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerAttackData;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject HPBar;

    [SerializeField]
    private GameObject MPBar;

    [SerializeField]
    private GameObject DashCount1;

    [SerializeField]
    private GameObject DashCount2;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Image[] skillCoolTime;

    [SerializeField]
    private Image[] skill;

    [SerializeField]
    private PlayerActionMgr playerActionMgr;

    private BuffManager buffManager;

    void Start()
    {
        buffManager = GetComponent<BuffManager>();
    }

    void Update()
    {
        CheckPlayerDashCount();
        CheckPlayerUseSkill();
        CheckPlayerDebuff();
    }

    void CheckPlayerDashCount() 
    {
        switch (playerController.DashCount) 
        {
            case 0:
                DashCount1.SetActive(false);
                break;
            case 1:
                DashCount1.SetActive(true);
                DashCount2.SetActive(false);
                break;
            case 2:
                DashCount2.SetActive(true);
                break;
        }
    }

	void CheckPlayerUseSkill()
	{
        skillCoolTime[0].fillAmount = 1 - AttackTable[101].curTime / AttackTable[101].cTime;

        skillCoolTime[1].fillAmount = 1 - AttackTable[102].curTime / AttackTable[102].cTime;

        if(PlayerAttackData.AttackTable[101].curTime == 0)
            skillCoolTime[0].gameObject.SetActive(false);
        else
            skillCoolTime[0].gameObject.SetActive(true);


        if(PlayerAttackData.AttackTable[102].curTime == 0)
            skillCoolTime[1].gameObject.SetActive(false);
        else        
            skillCoolTime[1].gameObject.SetActive(true);


        if(playerActionMgr.CanSkill3)
            skill[2].gameObject.SetActive(true);
		else
            skill[2].gameObject.SetActive(false);
	}

	void CheckPlayerDebuff() 
    {
        if (playerController.GetComponent<PlayerEffectController>().DarkDebuff == true) 
        {
            buffManager.DarkDebuff.SetActive(true);
            buffManager.DarkDebuff.transform.Find("Count").GetComponent<Text>().text = buffManager.DarkDebuffCount.ToString();
        }
        else 
        {
            buffManager.DarkDebuff.SetActive(false);
        }
    }
}
