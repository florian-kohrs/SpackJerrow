using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationController<T,J>
{

    void StartAnim(IAnimation<T, J> animation);

    void StartAnimAt(IAnimation<T, J> animation, float t);

    void PlayBackTime(float newFactor);

    void PauseAnim();

}
