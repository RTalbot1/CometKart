
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FinishLine : MonoBehaviour
{
    //Start is called before the first frame update
    private int collisionCount = 0;
    private bool playerComplete = false;
    public int playerPos = 0;
    public void OnTriggerEnter(Collider other)
    {
        // Update character's place in the game manager
        string name = other.name;
        if(name.Equals("temoc_with_car") || name.Equals("tobor_with_car")|| name.Equals("new_enarc_with_car")|| name.Equals("temocOLD_with_car"))
        {
            Debug.Log("PASSED LINE FINISH LINE"+ other.name);
            collisionCount++;
        }
        
       if (collisionCount >=5)
       {
            if(other.tag == "Player")
            {
                other.GetComponent<CarController>().enabled = false;
                playerComplete = true;
                playerPos = collisionCount;
            }
            else{
                other.GetComponent<CarAIController>().enabled = false;
            }
       }
    }
    public bool GetPlayerComplete()
    {
        return playerComplete;
    }
}
