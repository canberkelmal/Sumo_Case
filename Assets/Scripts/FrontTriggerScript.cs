using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTriggerScript : MonoBehaviour
{
    PlayerScript ps;
    void Start()
    {
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    //Hits the object more than normal collission if the character hit with face
    void OnTriggerEnter(Collider other)
    {
        if(( other.CompareTag("Player") || other.CompareTag("NPC") ) && gameObject.name == "FrontTrigger" && other.gameObject.name != "FrontTrigger")
        {
            Debug.Log(gameObject.name + " hit to " + other.gameObject.name);
            other.GetComponent<Rigidbody>().AddForce( (other.transform.position - transform.parent.position) * ps.bounceConstant/1.2f, ForceMode.Impulse);
        }
        else if (other.gameObject.name == "FrontTrigger" && gameObject.name == "FrontTrigger")
        {
            other.transform.parent.GetComponent<Rigidbody>().AddForce((other.transform.parent.position - transform.position) * ps.bounceConstant / 1.2f, ForceMode.Impulse);
        }


        if (other.gameObject.CompareTag("Player") && gameObject.transform.parent.CompareTag("NPC"))
        {
            other.gameObject.GetComponent<PlayerScript>().throwedBy = transform.parent.gameObject;
        }
        else if (other.gameObject.CompareTag("NPC") && gameObject.transform.parent.CompareTag("NPC"))
        {
            other.gameObject.GetComponent<NPCScript>().throwedBy = transform.parent.gameObject;
        }
        else if (other.gameObject.CompareTag("NPC") && gameObject.transform.parent.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerScript>().throwedBy = transform.parent.gameObject;
        }

    }
}
