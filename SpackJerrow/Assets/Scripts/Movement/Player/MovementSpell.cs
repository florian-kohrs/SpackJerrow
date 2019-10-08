using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StaminaRegeneration))]
public class MovementSpell : ScriptableObject
{

    public DirectionMode direction;

    public ForceMode forceMode;

    public float cost;

    public float cooldown;

    public float power;

    public InputTrigger input;

    public string buttonTriggerName;

    public bool useDeltaTime;

    public bool normalizeDirection;

    public bool canBeCastInAir;

}

public enum InputTrigger { OnDown, Up, Pressed, Permanent }


public enum DirectionMode
{
    WorldUp, Up, WorldDown, Down, Forward,
    LookDirectionForward, Left, LookDirectionLeft, Right, LookDirectionRight, Back, LookDirectionBack,
    InputAxis, TransformedInputAxis2D, TransformedInputAxis
}

