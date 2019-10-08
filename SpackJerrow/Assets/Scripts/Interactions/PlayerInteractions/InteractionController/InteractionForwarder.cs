using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionForwarder : MonoBehaviour
{

    public BaseInteraction rootInteraction;

    public BaseInteraction InteractionTarget => rootInteraction;

}
