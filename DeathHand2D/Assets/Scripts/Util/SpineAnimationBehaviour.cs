using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimationBehaviour : StateMachineBehaviour
{
	[SerializeField]
	private AnimationClip motion;
	private string animationClip;
	bool loop;

	[SerializeField]
	private int layer = 0;
	[SerializeField]
	private float timeScale = 1.0f;

	private SkeletonAnimation skeletonAnimation;
	private Spine.AnimationState spineAnimationState;
	private Spine.TrackEntry trackEntry;

	private void Awake()
	{
		if(motion != null)
			animationClip = motion.name;
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(skeletonAnimation == null)
		{
			skeletonAnimation = animator.GetComponent<SkeletonAnimation>();
			spineAnimationState = skeletonAnimation.state;
		}

		if(animationClip != null)
		{
			loop = stateInfo.loop;
			trackEntry = spineAnimationState.SetAnimation(layer, animationClip, loop);
			trackEntry.TimeScale = timeScale;
		}
	}
}
