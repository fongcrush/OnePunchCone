using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerAttack : MonoBehaviour
{
	public abstract void Begin();

	public abstract void UpdateAttack();

	public abstract void End();

	public abstract void Quit();
}