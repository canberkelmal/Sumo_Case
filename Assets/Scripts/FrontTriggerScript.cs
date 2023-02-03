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

    // Update is called once per frame
    void Update()
    {
        
    }

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
        
    }
}
