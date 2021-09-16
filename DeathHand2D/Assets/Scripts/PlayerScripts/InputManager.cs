using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class InputManager : MonoBehaviour
{
    public static float hAxis, vAxis;

    static void UpdateInput()
	{
        // 이동
        if(Input.GetKey(KeyCode.LeftArrow))
            curArrowKey = ArrowKey.Left;

        if(Input.GetKey(KeyCode.RightArrow))
            curArrowKey = ArrowKey.Right;

        if(Input.GetKey(KeyCode.UpArrow))
            curArrowKey = ArrowKey.Up;

        if(Input.GetKey(KeyCode.DownArrow))
            curArrowKey = ArrowKey.Down;

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        // 액션
        if(Input.GetKeyDown(KeyCode.Z))
            curActionKey = ActionKey.Z;

        if(Input.GetKeyDown(KeyCode.X))
            curActionKey = ActionKey.X;

        if(Input.GetKeyDown(KeyCode.C))
            curActionKey = ActionKey.C;

        if(Input.GetKeyDown(KeyCode.LeftShift))
            curActionKey = ActionKey.LeftShift;
    }

	private void Update()
	{
        UpdateInput();
	}
}
