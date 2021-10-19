using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimationBehaviour : StateMachineBehaviour
{
	[SerializeField]
	private AnimationClip motion;

	[SerializeField]
	private int layer = 0;

	[SerializeField]
	private float timeScale = 1.0f;

    private SkeletonAnimation skeletonAnimation;

    private Spine.AnimationState spineAnimationState;

    private Spine.TrackEntry trackEntry;

    private string animationClip;

    private bool loop;

    private float normalizedTime;

    private void Awake()
	{

        if(motion != null)
			animationClip = motion.name;
        loop = motion.isLooping;
	}

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(skeletonAnimation == null)
        {
            skeletonAnimation = animator.GetComponentInChildren<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.state;
        }

        trackEntry = spineAnimationState.SetAnimation(layer, animationClip, loop);
        trackEntry.TimeScale = timeScale;
        normalizedTime = 0f;        
    }

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;
        //if(!loop && normalizedTime >= exitTime)
        //{
        //	animator.SetTrigger("transition");
        //}
    }
}
