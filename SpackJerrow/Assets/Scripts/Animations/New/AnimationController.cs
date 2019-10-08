using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationController<T,J> : MonoBehaviour, IAnimationController<T,J>
{

    protected T currentAnimation;

    protected IEnumerator animationUpdater;

    public abstract void PauseAnim();
    public abstract void PlayBackTime(float newFactor);
    public abstract void StartAnim(IAnimation<T, J> animation);
    public abstract void StartAnimAt(IAnimation<T, J> animation, float t);
}
