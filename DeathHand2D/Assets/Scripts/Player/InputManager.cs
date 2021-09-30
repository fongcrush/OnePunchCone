using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;

public class InputManager : MonoBehaviour
{
    private static ArrowKey arrowKey = ArrowKey.None;
    public static ArrowKey curArrowKey { get { return arrowKey; } }

    private static ActionKey actionKey = ActionKey.None;
    public static ActionKey curActionKey { get { return actionKey; } }

    public static CharacterDirection characterDirection = CharacterDirection.Right;

    public static float hAxis, vAxis;

    private void Awake()
	{
        hAxis = vAxis = 0;
    }

	private void Update()
    {
        if(playerState != PlayerState.Dead)
        {
            if(actionState == ActionState.None)
                UpdateMoveInput();
            UpdateActionInput();
        }
    }

    void UpdateMoveInput()
	{
        // 이동
        if(Input.GetKey(KeyCode.LeftArrow)) { arrowKey = ArrowKey.Left; }

        else if(Input.GetKey(KeyCode.RightArrow)) { arrowKey = ArrowKey.Right; }

        else if(Input.GetKey(KeyCode.UpArrow)) { arrowKey = ArrowKey.Up; }

        else if(Input.GetKey(KeyCode.DownArrow)) { arrowKey = ArrowKey.Down; }

        else { arrowKey = ArrowKey.None; }

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
    }

    void UpdateActionInput()
    {
        // 액션
        if(Input.GetKeyDown(KeyCode.Z)) { actionKey = ActionKey.Z;}

        else if(Input.GetKeyDown(KeyCode.X)) { actionKey = ActionKey.X;}

        else if(Input.GetKeyDown(KeyCode.C)) { actionKey = ActionKey.C;}

        else if(Input.GetKeyDown(KeyCode.LeftShift)) { actionKey = ActionKey.LeftShift;}

        else { actionKey = ActionKey.None; }
    }
}
