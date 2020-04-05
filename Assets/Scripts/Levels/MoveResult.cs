using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO will likely eventually want more than just an enum here -
//need to know what type of enemy, item, etc.
public enum MoveResult
{
    NOTHING,
    STAIRSDOWN,
    ITEMPICKUP,
    CHESTOPEN,
    BATTLE
}
