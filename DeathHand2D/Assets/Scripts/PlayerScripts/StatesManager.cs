using UnityEngine;

public class StatesManager
{
    public static PlayerState playerState;
    
    public static CharacterDirection characterDirection;
    
    public static ArrowKey curArrowKey;

    public static ActionKey curActionKey;
    
    public static MoveMode moveMode;

    public static ActionState actionState;

    public enum PlayerState
    {
        Normal,
        Action,
		Dead
	}

	public enum MoveMode
	{
        Idle,
        Walk,
        Run
	}

	public enum CharacterDirection
    {
        Left,
        Right
    }

    public enum ArrowKey
    {
        None,
        Left,
        Right,
        Up,
		Down
	}

    public enum ActionKey
    {
        None,
        Z,
        X,
        C,
        LeftShift
    }

	public enum ActionState
	{
        None,
		Attack,
		Dash
	}
    public static bool LeftOrRight()
    {
        if(characterDirection == CharacterDirection.Left)
            return true;
        else
            return false;
    }
}
