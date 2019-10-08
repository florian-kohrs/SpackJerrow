using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackAnimationDeprecated
{

    public float colliderActivateDelay = 0.1f;

    public List<SceneAnimationPart> animations;

    private int currentIndex;

    private IEnumerator currentRotateAnim;

    private IEnumerator currentMoveAnim;

    private MonoBehaviour b;

    private Transform t;

    private System.Action onFinish;

    public static AttackAnimationDeprecated PlayTransitionTo(MonoBehaviour source, Vector3 localPosition, Vector3 localEuler, float time)
    {
        AttackAnimationDeprecated animation = new AttackAnimationDeprecated();
        animation.animations = new List<SceneAnimationPart>() { new SceneAnimationPart() { localEuler = localEuler, localPosition = localPosition, timeNeeded = time } };
        animation.PlayAnimation(source);
        return animation;
    }

    public void PlayAnimation(MonoBehaviour b, System.Action onFinish = null)
    {
        PlayAnimation(b, b.transform, onFinish);
    }

    public void PlayAnimation(MonoBehaviour b, Transform t, System.Action onFinish = null)
    {
        this.b = b;
        this.t = t;
        this.onFinish = onFinish;
        PlayNextPart(currentIndex);
    }

    private void PlayNextPart(int index)
    {
        currentIndex = index;
        if (currentIndex < animations.Count)
        {
            SceneAnimationPart currentPart = animations[currentIndex];
            currentRotateAnim = SmoothTransformation<Vector3>.SmoothRotateAngleEuler(t.localEulerAngles, currentPart.localEuler, currentPart.timeNeeded, v => t.localEulerAngles = v, () =>
               PlayNextPart(currentIndex + 1));

            currentMoveAnim = SmoothTransformation<Vector3>.SmoothRotateAngleEuler(t.localPosition, currentPart.localPosition, currentPart.timeNeeded, v => t.localPosition = v);
            b.StartCoroutine(currentRotateAnim);
            b.StartCoroutine(currentMoveAnim);
        }
        else
        {
            onFinish?.Invoke();
            currentIndex = 0;
        }
    }


    public void Abort()
    {
        if (currentRotateAnim != null)
        {
            b.StopCoroutine(currentRotateAnim);
        }
        if (currentMoveAnim != null)
        {
            b.StopCoroutine(currentMoveAnim);
        }
    }

}
