using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    IEnvironment curTriggerObject;

    protected const int PlayerLayer = 20;
    protected const int EnemyLayer = 10;

    private void Awake()
    {
        EnvironmentData.UpdateCSVData();
        // GetTableValue
        // Debug.Log(EnvironmentData.EnvironmentTable["Bush"].name);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        curTriggerObject = GetComponent<IEnvironment>();
        curTriggerObject.Stay(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        curTriggerObject = GetComponent<IEnvironment>();
        curTriggerObject.Exit(collision);
    }
}

abstract public class IEnvironment : Environment
{
    public abstract void Stay(Collider2D collision);
    public abstract void Exit(Collider2D collision);
}