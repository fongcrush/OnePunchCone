using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    EnvironmentInfo info;
    Bush bush;
    DeadManHand deadManHans;

    public string environmentName;

    protected const int PlayerLayer = 20;
    protected const int EnemyLayer = 10;

    private void Awake()
    {
        EnvironmentData.UpdateCSVData();
        bush = GetComponent<Bush>();
        deadManHans = GetComponent<DeadManHand>();
        
    }

    void Start()
    {
        if(environmentName != "Parent")
            info = EnvironmentData.EnvironmentTable[environmentName];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (environmentName) 
        {
            case "Bush":
                bush.TriggerEnterBush(collision, true);
                break;
            case "DeadManHand":
                deadManHans.TriggerEnterDeadManHand(collision, true);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (environmentName)
        {
            case "Bush":
                bush.TriggerEnterBush(collision, false);
                break;
            case "DeadManHand":
                deadManHans.TriggerEnterDeadManHand(collision, false);
                break;
        }
    }
}