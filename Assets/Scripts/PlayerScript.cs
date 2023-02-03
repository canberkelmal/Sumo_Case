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
    GameObject cam, controlTarget, rotationController;
    NavMeshAgent playerAgent;
    Rigidbody playerRb;

    [TabGroup("GamePlay")]
    public float playerSpeed;
    [TabGroup("GamePlay")]
    public float NPCSpeed;
    [TabGroup("GamePlay")]
    public float playerSpeedLimit;
    [TabGroup("GamePlay")]
    public float NPCSpeedLimit;
    [TabGroup("UI")]
    public Joystick joystick;
    [TabGroup("GameData")]
    public float playerRotateSpeed;
    [TabGroup("GameData")]
    public float cameraFallowSensivity;
    [TabGroup("GameData")]
    public float bounceConstant;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        controlTarget = GameObject.Find("PlayerDirector");
        rotationController = GameObject.Find("RotationController");
        playerRb = transform.GetComponent<Rigidbody>();
        camOfs = transform.position - cam.transform.position;
        controlTarget.transform.position = transform.position + transform.forward;
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
        rotationController.transform.position = transform.position;
        rotationController.transform.LookAt(controlTarget.transform);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationController.transform.rotation, playerRotateSpeed* 100 * Time.deltaTime) ;
        if(playerRb.velocity.sqrMagnitude < playerSpeedLimit)
        {

            playerRb.AddForce(transform.forward * playerSpeed * Time.deltaTime, ForceMode.Force);
        }
        //transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * 5, playerSpeed * Time.deltaTime);
        //playerAgent.destination = controlTarget.transform.position;

        //playerController.Move(moveVector * playerSpeed * Time.deltaTime);

        //Restart the scene when "R" key is pressed.(to use during developing and testing)
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("NPC") && gameObject.name == "Player")
        {
            Debug.Log("Player hit to " + other.gameObject.name);
            other.gameObject.GetComponent<Rigidbody>().AddForce(( other.gameObject.transform.position - transform.position ) * bounceConstant / 1.2f, ForceMode.Impulse);
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
