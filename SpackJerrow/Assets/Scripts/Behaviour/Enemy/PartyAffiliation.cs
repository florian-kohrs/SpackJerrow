using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyAffiliation : SaveableMonoBehaviour
{

    public enum PartyName { Unkown, Pirate, French, Animal, Fiend }

    [Save]
    public PartyName party;

}
