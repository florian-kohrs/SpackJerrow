using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceController : SaveableMonoBehaviour
{

    public List<GameObject> activateOnPlay;

    public List<GameObject> deactivateOnPlay;

    //public List<MonoBehaviour> disableOnPlay;

    //public List<MonoBehaviour> disableOnFinish;

    //public List<MonoBehaviour> enableOnStart;

    //public List<MonoBehaviour> enableOnFinish;

    public List<GameObject> activateOnFinish;

    public List<Object> deleteOnFinish;

    public Transform lookAtPoint;
    
    public List<SequencePoints> lookAtSequences;

    public Transform mainPoint;

    public Camera spectator;

    [Save]
    public bool skipSequence;

    public List<SequencePoints> mainSequences;

    public void OnStart()
    {
        if (skipSequence)
        {
            OnFinish();
        }
        else
        {
            enabled = true;
            skipSequence = true;
            foreach (GameObject g in activateOnPlay)
            {
                g.SetActive(true);
            }

            foreach (GameObject g in deactivateOnPlay)
            {
                g.SetActive(false);
            }

            Starting();
            PlayLookAt(0);
            PlayMainPoint(0);
        }
    }

    protected virtual void Starting() { }

    private void Start()
    {
        OnStart();
    }

    protected virtual void Update()
    {
        spectator.transform.LookAt(lookAtPoint);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnFinish();
        }
    }

    public void PlayMainPoint(int index)
    {
        if (index < mainSequences.Count)
        {
            int newIndex = index + 1;
            mainSequences[index].StartSequencePart(this, mainPoint, () => PlayMainPoint(newIndex));
        }
        else
        {
            CheckOnFinish();
        }
    }

    public void PlayLookAt(int index)
    {
        if(index < lookAtSequences.Count)
        {
            int newIndex = index + 1;
            lookAtSequences[index].StartSequencePart(this, lookAtPoint, () => PlayLookAt(newIndex));
        }
        else
        {
            CheckOnFinish();
        }
    }

    private int finishCounts = 0;

    private void CheckOnFinish()
    {
        finishCounts++;
        ///call finish when both animations are done
        if(finishCounts == 2)
        {
            OnFinish();
        }
    }

    public virtual void IsFinished() { }

    private void OnFinish()
    {

        foreach(SequencePoints p in lookAtSequences)
        {
            p.Abort();
        }

        foreach (SequencePoints p in mainSequences)
        {
            p.Abort();
        }

        enabled = false;
        
        foreach (Object o in deleteOnFinish)
        {
            Destroy(o);
        }

        foreach (GameObject g in activateOnFinish)
        {
            if (g != null)
            {
                g.SetActive(true);
            }
        }
        spectator.gameObject.SetActive(false);
        enabled = false;
        IsFinished();
    }

}
