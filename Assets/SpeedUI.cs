using UnityEngine.UI;
using UnityEngine;

public class SpeedUI : MonoBehaviour
{

    public Text speed_text;
    [SerializeField] Car car;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        speed_text.text = ((int)car.currentSpeed).ToString();
    }
}
