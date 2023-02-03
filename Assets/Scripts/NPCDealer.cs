using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDealer : MonoBehaviour
{
    bool isTargetNPCFallowed = false;
    GameObject tempTarget;
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            tempTarget = transform.GetChild(UnityEngine.Random.Range(0, transform.childCount)).gameObject;
            isTargetNPCFallowed = tempTarget.GetComponent<NPCScript>().fallowed;
            if(isTargetNPCFallowed)
            {
                while (tempTarget.GetComponent<NPCScript>().fallowed) {
                    tempTarget = transform.GetChild(UnityEngine.Random.Range(0, transform.childCount)).gameObject;
                }
                transform.GetChild(i).GetComponent<NPCScript>().targetNPC= tempTarget;
                tempTarget.GetComponent<NPCScript>().fallowed = true;
            }
            else
            {
                transform.GetChild(i).GetComponent<NPCScript>().targetNPC = tempTarget;
            }
        }
    }
}
