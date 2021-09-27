using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject HPBar;
    public GameObject MPBar;
    public GameObject Skill1;
    public GameObject Skill2;
    public GameObject Skill3;
    public GameObject DashCount1;
    public GameObject DashCount2;

    public GameObject playerGameObject;
    PlayerController playerComponent;
    BuffManager buffManager;

    void Start()
    {
        playerComponent = playerGameObject.GetComponent<PlayerController>();
        buffManager = GetComponent<BuffManager>();
    }

    void Update()
    {
        CheckPlayerDashCount();
        //CheckPlayerUseSkill();
        CheckPlayerDebuff();
    }

    void CheckPlayerDashCount() 
    {
        switch (playerComponent.DashCount) 
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

    //void CheckPlayerUseSkill() 
    //{
    //    if (PlayerAttackManager.skillTable[101].curTime != 0)
    //    {
    //        Skill1.GetComponent<Image>().color = Color.black;
    //    }
    //    else 
    //    {
    //        Skill1.GetComponent<Image>().color = Color.white;
    //    }
    //    if(PlayerAttackManager.skillTable[111].curTime != 0) 
    //    {
    //        Skill2.GetComponent<Image>().color = Color.black;
    //        if (playerComponent.canskill3 == true)
    //        {
    //            Skill3.SetActive(true);
    //        }
    //        else
    //        {
    //            Skill3.SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        Skill2.GetComponent<Image>().color = Color.white;
    //    }       
    //}

    void CheckPlayerDebuff() 
    {
        if (playerGameObject.GetComponent<PlayerEffectController>().DarkDebuff == true) 
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
