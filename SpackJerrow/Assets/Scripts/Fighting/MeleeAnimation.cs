using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeleeAnimation
{

    public List<VectorAnimation> positions;

    public List<VectorAnimation> rotations;

    private bool pFinish;

    private bool rFinish;

    private System.Action onFinish;
    private MonoBehaviour m;
    private Transform animatedTarget;


    private IEnumerator rotateEnumerator;
    private IEnumerator positionEnumerator;

    public void CancelAnimation()
    {
        m.StopCoroutine(rotateEnumerator);
        m.StopCoroutine(positionEnumerator);
        pFinish = false;
        rFinish = false;
        onFinish = null;
    }

    public void CancelAnimationIfStarted()
    {
        if (m != null)
        {
            CancelAnimation();
        }
    }

    public void PlayAttackAnim(MonoBehaviour animator, Transform animatedTarget, System.Action onFinish = null)
    {
        this.m = animator;
        this.onFinish = onFinish;
        this.animatedTarget = animatedTarget;
        PlayRotationAnim(rotations, 0);
        PlayTranslationAnim(positions, 0);
    }

    public void PlayRotationAnim(List<VectorAnimation> vs, int i)
    {
        if (vs.Count > i)
        {
            m.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(vs[i].start, vs[i].end, vs[i].time,
                (v) => animatedTarget.localEulerAngles = v,
                () =>
                {
                    PlayRotationAnim(vs, i + 1);
                }));
        }
        else
        {
            rFinish = true;
            CheckIfDone();
        }
    }

    public void PlayTranslationAnim(List<VectorAnimation> vs, int i)
    {
        if (vs.Count > i)
        {
            m.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateEuler(vs[i].start, vs[i].end, vs[i].time,
                (v) => animatedTarget.localPosition = v,
                () =>
                {
                    PlayTranslationAnim(vs, i + 1);
                }));
        }
        else
        {
            pFinish = true;
            CheckIfDone();
        }
    }

    private void CheckIfDone()
    {
        if(rFinish && pFinish)
        {
            rFinish = false;
            pFinish = false;
            m = null;
            animatedTarget = null;
            onFinish?.Invoke();
        }
    }

}
