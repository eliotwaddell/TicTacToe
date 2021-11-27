using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI victory_display;

    // Update is called once per frame
    public void DisplayWinner(int winner)
    {
        if(winner == 0)
        {
            victory_display.SetText("X Wins!");
        }
        else if(winner == 1)
        {
            victory_display.SetText("0 Wins!");
        }
        else
        {
            victory_display.SetText("It's a draw!");
        }
    }

    public void ResetText()
    {
        victory_display.SetText("");
    }
}
