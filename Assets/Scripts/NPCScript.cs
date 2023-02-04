using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class NPCScript : MonoBehaviour
{
    PlayerScript ps;
    Rigidbody NPCRb;
    bool tempOutOfArea = false;

    [HideInInspector]
    public GameObject NPCs, targetNPC;
    [HideInInspector]
    public bool fallowed = false, outOfArea = false;
    [HideInInspector]
    public GameObject throwedBy;
    [HideInInspector]
    public int NPCScore = 100;

    void Start()
    {
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
        NPCs = GameObject.Find("NPCs");
        NPCRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(!outOfArea)
        {
            NPCController();
        }
    }

    //Set the throwed NPC as out of area and transfer the score
    public void OutOfArea()
    {
        if (throwedBy.CompareTag("Player"))
        {
            ps.IncreaseScore(this.NPCScore);
        }
        else if (throwedBy.CompareTag("NPC"))
        {
            IncreaseTheKiller();
        }
        transform.SetParent(null);
        outOfArea = true;
        NPCRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //Destroy the NPC after 0.2 second
        Destroy(gameObject, 0.2f);
    }

    //Turns and runs to a available NPC
    void NPCController()
    {
        tempOutOfArea = targetNPC.gameObject != ps.gameObject ? targetNPC.GetComponent<NPCScript>().outOfArea : false; 
        if (targetNPC!= null && !tempOutOfArea && targetNPC != gameObject)
        {
            transform.LookAt(targetNPC.transform.position);
            if (NPCRb.velocity.sqrMagnitude < ps.NPCSpeedLimit)
            {
                NPCRb.AddForce(transform.forward * ps.NPCSpeed * Time.deltaTime, ForceMode.Force);
            }
        }
        else if(NPCs.transform.childCount> 1)
        {
            targetNPC = NPCs.transform.GetChild(UnityEngine.Random.Range(0, NPCs.transform.childCount)).gameObject; 
        }
        else
        {
            targetNPC = ps.gameObject;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (( other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("NPC") ) && gameObject.name == "NPC")
        {
            Debug.Log("NPC hit to " + other.gameObject.name);
            other.gameObject.GetComponent<Rigidbody>().AddForce((other.gameObject.transform.position - transform.position) * ps.bounceConstant/1.2f, ForceMode.Impulse);
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerScript>().throwedBy = gameObject;
            }
            else if (other.gameObject.CompareTag("NPC"))
            {
                other.gameObject.GetComponent<NPCScript>().throwedBy = gameObject;
            }
        }

    }

    //Adds point of the throwed out NPC
    void IncreaseTheKiller()
    {
        throwedBy.GetComponent<NPCScript>().NPCScore += this.NPCScore;
    }
}
