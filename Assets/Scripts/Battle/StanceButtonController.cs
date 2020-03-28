using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceButtonController : MonoBehaviour
{
    public PlayerRollGenerator playerRoll;
    private List<StanceButton> buttons;

    private void Awake()
    {
        buttons = new List<StanceButton>();
    }

    public void RegisterStanceButton(StanceButton button)
    {
        buttons.Add(button);
    }

    public void SelectStance(StanceButton button)
    {
        foreach (StanceButton b in buttons)
        {
            if (b != button)
            {
                b.SetUnselected();
            }
        }
        //playerRoll.SetStance(button.stanceName);
    }
}
