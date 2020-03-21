using UnityEngine;

public abstract class RollResultModifier : MonoBehaviour
{
    public abstract RollResult apply(RollResult initial);
}
