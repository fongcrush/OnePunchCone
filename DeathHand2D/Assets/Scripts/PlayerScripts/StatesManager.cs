 using UnityEngine;

public class StatesManager
{
    public static PlayerState playerState = PlayerState.Idle;
    
    public static CharacterDirection characterDirection = CharacterDirection.Right;
    
    public static ArrowKey curArrowKey = ArrowKey.None;

    public static ActionKey curActionKey = ActionKey.None;
    
    public static MoveMode moveMode = MoveMode.Walk;

    public static ActionState actionState = ActionState.None;

    public static AttackState attackState = AttackState.None;

    public enum PlayerState
    {
        Idle,
        Move,
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

    public enum AttackState
    {
        None,
        AAttack,
        Skill1,
        Skill2,
        Skill3
    }

	public enum ActionState
	{
        None,
        Ready,
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
