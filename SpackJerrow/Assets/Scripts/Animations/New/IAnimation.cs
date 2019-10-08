using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">t is animated value</typeparam>
/// <typeparam name="J">j is the object the animated value is applied to</typeparam>
public interface IAnimation<T,J>
{

    void StartAnimationAt(float time);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <returns>returns true when the animation is finished</returns>
    bool UpdateAnim(float deltaTime);
    
}
