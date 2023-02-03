using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWallScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("NPC"))
        {
            other.GetComponent<NPCScript>().OutOfArea();
        }
    }
}
