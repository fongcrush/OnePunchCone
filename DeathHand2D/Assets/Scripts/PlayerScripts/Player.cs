using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using static StatesManager;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public StatusManager stat;
    [HideInInspector]
    public Actor gameManager;

    private IPlayerBehaviour curPlayerBehaviour;
    private PlayerMoveBehaviour move;
    private PlayerActionBehaviour action;

    [SerializeField]
    private int dashCount;

    [SerializeField]
    private bool dashGodMode;
    public bool DashGodMode { get { return dashGodMode; } set { dashGodMode = value; } }


    public PlayerState displayPlayerState;
    public CharacterDirection displayCharacterDir;
    public ArrowKey displayCurArrowKey;
    public ActionKey displayCurActionKey;
    public MoveMode displayCurMoveMode;
    public ActionState displayActionState;

    private void Awake()
    {
        move               = new PlayerMoveBehaviour(this);
        action             = new PlayerActionBehaviour(this);

        gameManager        = GameObject.Find("@GM").GetComponent<Actor>();

        stat               = new StatusManager(100, 100, 50);
        dashGodMode        = false;

        characterDirection = CharacterDirection.Right;
        dashCount          = 2;
    }

    public PlayerState CurrentState()
	{
        return playerState;
	}

    // Start is called before the first frame update
    void Start()
    {
        curPlayerBehaviour = move;
        playerState = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerState == PlayerState.Move)
            curPlayerBehaviour = move;
        else if(playerState == PlayerState.Action)
            curPlayerBehaviour = action;

        curPlayerBehaviour.Update();

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
	//private void OnTriggerStay2D(Collider2D collision)
	//{
	//    if(collision.gameObject.tag == "PerfectTiming")
	//    {
	//        if(!isTiming)
	//        {
	//            isTiming = true;
	//        }
	//    }
	//}

	//private void OnTriggerExit2D(Collider2D collision)
	//{
	//    if(collision.gameObject.tag == "PerfectTiming")
	//    {
	//        isTiming = false;
	//    }
	//}

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

	public int DashCount
    {
        get { return dashCount; }
        set { dashCount = value; }
    }
}