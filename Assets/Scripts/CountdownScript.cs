using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class CountdownScript : MonoBehaviour
{
    public GameObject Countdown;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine (StartCountdown ());
    }

    IEnumerator StartCountdown ()
    {
        yield return new WaitForSeconds(0.5f);
        Countdown.GetComponent<TextMeshProUGUI>().text = "Test";
        Countdown.SetActive(true);
        //yield return new WaitForSeconds(1f);
        Countdown.SetActive(false);
        Countdown.GetComponent<TextMeshProUGUI>().text = "2";
        Countdown.SetActive(true);
        yield return new WaitForSeconds(1f);
        Countdown.SetActive(false);
        Countdown.GetComponent<TextMeshProUGUI>().text = "1";
        Countdown.SetActive(true);
        yield return new WaitForSeconds(1f);
        Countdown.SetActive(false);
        Countdown.GetComponent<TextMeshProUGUI>().text = "GO!";
        Countdown.SetActive(true);
        yield return new WaitForSeconds(1f);
        Countdown.SetActive(false);
    }
}
