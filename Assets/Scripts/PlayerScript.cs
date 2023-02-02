using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    CharacterController playerController;
    Vector3 moveVector;

    [TabGroup("GamePlay")]
    public float playerSpeed;
    [TabGroup("UI")]
    public Joystick joystick;
    // Start is called before the first frame update
    void Start()
    {
        playerController= GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputsController();
    }

    void InputsController()
    {



        moveVector = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        playerController.Move(moveVector * playerSpeed * Time.deltaTime);



        //Restart the scene when "R" key is pressed.(to use during developing and testing)
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Time.timeScale = 1;
    }
}
