using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    CharacterController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController= GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputController();
    }

    void InputController()
    {
        throw new NotImplementedException();
    }
}
