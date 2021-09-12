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

    public GameObject player;


    void Start()
    {

    }

    void Update()
    {
        CheckPlayerDashCount();
        CheckPlayerUseSkill();
    }

    void CheckPlayerDashCount() 
    {
        switch (player.GetComponent<PlayerController>().DashCount) 
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
        if (player.GetComponent<Player>().canskill1 == false)
        {
            Skill1.GetComponent<Image>().color = Color.black;
        }
        else 
        {
            Skill1.GetComponent<Image>().color = Color.white;
        }
        if (player.GetComponent<Player>().canskill2 == false) 
        {
            Skill2.GetComponent<Image>().color = Color.black; 
            Skill3.GetComponent<Image>().color = Color.black;
            if(player.GetComponent<Player>().canskill1 == false) 
            {
                Skill3.SetActive(false);
            }
        }
        else 
        {
            Skill2.GetComponent<Image>().color = Color.white;
            Skill3.GetComponent<Image>().color = Color.white;
        }
    }

}
