using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnCompleteLap : MonoBehaviour
{
    public GameObject LapText;

    private int lap = 0;

    void OnTriggerEnter()
    {
        Debug.Log("Test");
        if(LapText != null)
        {
            lap++;
            if(lap > 1)
            {
                LapText.GetComponent<TextMeshProUGUI>().text = "Lap: " + lap;
            }
        }
    }
}