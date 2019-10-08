using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllableCoroutine
{

    bool IsPlaying { get; }

    void Stop();

    void Finish();

    void Start(MonoBehaviour source);
    
}
