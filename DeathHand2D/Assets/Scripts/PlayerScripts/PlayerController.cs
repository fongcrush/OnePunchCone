using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using static PlayerStatesData;
using static InputManager;

public class PlayerController : MonoBehaviour
{
    private InputManager inputController;

    private PlayerActionMgr actionMgr;
    public PlayerActionMgr ActionMgr { get { return actionMgr; } }

    private PlayerMove move;

    [SerializeField]
    private int dashCount;
    public int DashCount { get { return dashCount; } set { dashCount = value; } }

    [SerializeField]
    private bool dashGodMode;
    public bool DashGodMode { get { return dashGodMode; } set { dashGodMode = value; } }


    [HideInInspector]
    public StatusManager stat;

    [HideInInspector]
    public Actor gameManager;

    public PlayerState displayPlayerState;
    public CharacterDirection displayCharacterDir;
    public ArrowKey displayCurArrowKey;
    public ActionKey displayCurActionKey;
    public MoveMode displayCurMoveMode;
    public ActionState displayActionState;

    private void Awake()
    {
        gameManager = GameObject.Find("@GM").GetComponent<Actor>();
        actionMgr = transform.Find("ActionManager").GetComponent<PlayerActionMgr>();
        move = GetComponent<PlayerMove>();

        stat = new StatusManager(100, 100, 50);
        characterDirection = CharacterDirection.Right;
        dashCount = 2;
        dashGodMode = false;
    }

    // Update is called once per frame
    void Update()
	{
        switch(playerState)
        {
        case PlayerState.Move:
            move.UpdateMove();
            break;
        case PlayerState.Action:
            actionMgr.UpdateAction();
            break;

        case PlayerState.Dead:

            break;
        }
		UpdateDisplayStates();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Arrow")
        {
            Destroy(collision.gameObject);
        }
    }

    private void UpdateDisplayStates()
    {
        displayPlayerState = playerState;
        displayActionState = actionState;
        displayCharacterDir = characterDirection;
        displayCurArrowKey = curArrowKey;
        displayCurActionKey = curActionKey;
        displayCurMoveMode = moveMode;
    }

    public bool LeftOrRight()
    {
        if(characterDirection == CharacterDirection.Left)
            return true;
        else
            return false;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.transform.tag == "Enemy") 
    //    {
    //        if (dashGodMode == false) 
    //        {
    //            //Hp -= 0.5f;
    //        }
    //    }
    //}
}