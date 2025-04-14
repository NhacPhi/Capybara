using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Test : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    TrackEntry trackEntry;
    private void Awake()
    {
        trackEntry = GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, AnimConstant.Move, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            trackEntry.Reverse = !trackEntry.Reverse;
        }
    }
}
