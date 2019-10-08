using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : SetController<BaseInteraction>
{

    private float INTERACTION_MAX_RANGE = 4;

    public GameObject interactionDisplayer;

    public Text interactionDescription;

    public Text interactionKeyCode;

    public Vector3 Direction => Camera.main.transform.forward;

    public Vector3 Source => transform.position;

    private Dictionary<KeyCode, List<BaseInteraction>> interactions = 
        new Dictionary<KeyCode, List<BaseInteraction>>();

    public void DisplayInteraction(BaseInteraction i)
    {
        interactionDisplayer.SetActive(true);
        interactionDescription.text = i.InteractionDescription;
    }

    public override void AddElement(BaseInteraction element)
    {
        List<BaseInteraction> bs;
        KeyCode interactKeyCode = element.InteractKeyCode;

        if (!interactions.TryGetValue(
            interactKeyCode, out bs))
        {
            bs = new List<BaseInteraction>();
            interactions.Add(interactKeyCode, bs);
        }
        bs.Add(element);
    }

    public override void RemoveElement(BaseInteraction element)
    {
        RemoveDelayed.Push(element);
    }

    private Stack<BaseInteraction> removeDelayed;

    private Stack<BaseInteraction> RemoveDelayed
    {
        get
        {
            if(removeDelayed == null)
            {
                removeDelayed = new Stack<BaseInteraction>();
            }
            return removeDelayed;
        }
    }

    private void Update()
    {
        if (interactions.Count == 0)
        {
            interactionDisplayer.SetActive(false);
        }
        else
        {
            foreach (KeyCode key in interactions.Keys)
            {
                DisplayInteraction(interactions[key][0]);
                if (Input.GetKeyDown(key))
                {
                    interactions[key][0].Interact(gameObject);
                }
            }
        }
        while (RemoveDelayed.Count > 0)
        {
            BaseInteraction element = RemoveDelayed.Pop();
            KeyCode interactKeyCode = element.InteractKeyCode;
            List<BaseInteraction> bs = interactions[interactKeyCode];
            bs.Remove(element);
            if (bs.Count == 0)
            {
                interactions.Remove(interactKeyCode);
            }
        }
        //RaycastHit hit;
        //Debug.DrawRay(Source, Direction, Color.red, 0.1f);
        //if (Physics.Raycast(Source, Direction, out hit, INTERACTION_MAX_RANGE))
        //{
        //    BaseInteraction b = hit.transform.GetComponent<InteractionForwarder>()?.InteractionTarget;
        //    if (b != null)
        //    {
        //        Debug.Log(b.InteractButtonName);
        //        Debug.Log(b.InteractKeyCode);
        //        if (b != null)
        //        {
        //            if (Input.GetKeyDown(b.InteractKeyCode))
        //            {
        //                b.Interact(gameObject);
        //            }
        //        }
        //    }
        //}
    }
}
