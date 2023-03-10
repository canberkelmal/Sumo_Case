using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWallScript : MonoBehaviour
{
    PlayerScript ps;
    void Start()
    {
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    //Detect the throwed objects
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " is out of the area!-!");
        if (other.CompareTag("NPC"))
        {
            ps.decreaseSumo(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            ps.GameOver("down");
        }
    }
}
