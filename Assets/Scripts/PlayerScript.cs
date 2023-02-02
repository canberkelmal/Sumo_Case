using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    CharacterController playerController;
    Vector3 moveVector = Vector3.zero;
    Vector3 moveVectorX, moveVectorZ, camOfs;
    GameObject cam, controlTarget;
    NavMeshAgent playerAgent;

    [TabGroup("GamePlay")]
    public float playerSpeed;
    [TabGroup("UI")]
    public Joystick joystick;
    [TabGroup("GameData")]
    public float cameraFallowSensivity;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        controlTarget = GameObject.Find("Control Target");
        camOfs = transform.position - cam.transform.position;
    }

    void FixedUpdate()
    {
        InputsController();
        CamController();
    }

    void InputsController()
    {
        if(joystick.Vertical != 0 || joystick.Horizontal != 0)
        {
            moveVector = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        }
        controlTarget.transform.position = transform.position + moveVector;
        transform.LookAt(controlTarget.transform);
        transform.position += transform.forward * Time.deltaTime * playerSpeed;
        //playerAgent.destination = controlTarget.transform.position;

        //playerController.Move(moveVector * playerSpeed * Time.deltaTime);

        //Restart the scene when "R" key is pressed.(to use during developing and testing)
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }
    
    void CamController()
    {
        //cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position - camOfs, cameraFallowSensivity * Time.deltaTime);
        cam.transform.position = transform.position - camOfs;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Time.timeScale = 1;
    }
}
