using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    public enum States 
    {
        Idle,
        Walk,
        Run,
        Desh,
        Attack,
        Skill1,
        Skill2,
        Skill3,
        Dead
    }

    [SerializeField]
    private States playerState = new States();

    private void Idle() 
    {
        playerState = States.Idle;
    }
    private void Walk()
    {
        playerState = States.Walk;
    }
    private void Run()
    {
        playerState = States.Run;
    }
    private void Desh()
    {
        playerState = States.Desh;
    }
    private void Attack()
    {
        playerState = States.Attack;
    }
    private void Skill1()
    {
        playerState = States.Skill1;
    }
    private void Skill2()
    {
        playerState = States.Skill2;
    }
    private void Skill3()
    {
        playerState = States.Skill3;
    }
    private void Dead()
    {
        playerState = States.Dead;
    }

    public string State 
    {
        get
        {
            return playerState.ToString();
        }
        set 
        {
            switch (value)
            {
                case "Idle":
                    Idle();
                    break;
                case "Walk":
                    Walk();
                    break;
                case "Run":
                    Run();
                    break;
                case "Desh":
                    Desh();
                    break;
                case "Attack":
                    Attack();
                    break;
                case "Skill1":
                    Skill1();
                    break;
                case "Skill2":
                    Skill2();
                    break;
                case "Skill3":
                    Skill3();
                    break;
                case "Dead":
                    Dead();
                    break;
                default:
                    Debug.LogWarning("Error: Can't Find String to Set State");
                    break;
            }
        }
    }
}
