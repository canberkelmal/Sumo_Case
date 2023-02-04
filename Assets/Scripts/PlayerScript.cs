using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    CharacterController playerController;
    Vector3 moveVector = Vector3.zero;
    Vector3 moveVectorX, moveVectorZ, camOfs;
    GameObject cam, controlTarget, rotationController;
    NavMeshAgent playerAgent;
    Rigidbody playerRb;
    float timeRemaining;
    [HideInInspector]
    public int score, sumoCounter;
    [HideInInspector]
    public GameObject throwedBy;

    [Title("Game Settings")]
    [TabGroup("GamePlay")]
    [ReadOnly]
    public bool isPlayerAlive = true;
    [TabGroup("GamePlay")]
    public int roundTime;

    [Title("Player Settings")]
    [TabGroup("GamePlay")]
    public float playerSpeed;
    [TabGroup("GamePlay")]
    public float playerSpeedLimit;

    [Title("NPC Settings")]
    [TabGroup("GamePlay")]
    public float NPCSpeed;
    [TabGroup("GamePlay")]
    public float NPCSpeedLimit;


    [Title("UI Texts")]
    [TabGroup("UI")]
    public Text timeTX;
    [TabGroup("UI")]
    public Text scoreTX;
    [TabGroup("UI")]
    public Text sumoCounterTX;

    [Title("UI Panels")]
    [TabGroup("UI")]
    public GameObject winPanel;
    [TabGroup("UI")]
    public GameObject losePanel;
    [TabGroup("UI")]
    public GameObject pausePanel;

    [Title("UI Others")]
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
        Time.timeScale = 0;

        cam = GameObject.Find("Main Camera");

        //rotationController always looks on controlTarget object
        controlTarget = GameObject.Find("PlayerDirector");

        //Player rotation is always set to this object's rotation
        rotationController = GameObject.Find("RotationController");

        playerRb = transform.GetComponent<Rigidbody>();

        //Set camera fallowing offset as designed on scene
        camOfs = transform.position - cam.transform.position;

        //This object's position altways set to player position
        controlTarget.transform.position = transform.position + transform.forward;
        timeRemaining = roundTime;        
    }

    void Update()
    {
        //Timer
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            GameOver("time");
        }

        //Restart the scene when "R" key is pressed.(to use during developing and testing)
        //Placed on update to fix detection bug
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    void FixedUpdate()
    {
        PlayerController();
        CamController();

        //Set timer text with timeRemaining value
        timeTX.text = ((int)timeRemaining).ToString();
    }

    //Controls player and player's control components
    void PlayerController()
    {
        //Set move vector if there is any joystick imput
        if(joystick.Vertical != 0 || joystick.Horizontal != 0)
        {
            moveVector = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        }

        //Set player controller components positions and rotations
        controlTarget.transform.position = transform.position + moveVector;
        rotationController.transform.position = transform.position;
        rotationController.transform.LookAt(controlTarget.transform);

        //Set players rotation according to applied force with joystick
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationController.transform.rotation, playerRotateSpeed* 100 * Time.deltaTime) ;

        //Add force to player according to playerSpeed if current speed magnitude lower than playerSpeedLimit
        if(playerRb.velocity.magnitude < playerSpeedLimit)
        {
            playerRb.AddForce(transform.forward * playerSpeed * Time.deltaTime, ForceMode.Force);
        }

    }
    void OnCollisionEnter(Collision other)
    {
        //Throw the hit NPC
        if (other.gameObject.CompareTag("NPC") && gameObject.name == "Player")
        {
            Debug.Log("Player hit to " + other.gameObject.name);
            other.gameObject.GetComponent<Rigidbody>().AddForce(( other.gameObject.transform.position - transform.position ) * bounceConstant / 1.2f, ForceMode.Impulse);
            other.gameObject.GetComponent<NPCScript>().throwedBy = gameObject;
        }
    }

    //Controls the camera according to player position
    void CamController()
    {
        cam.transform.position = transform.position - camOfs;
    }

    //Load the current scene
    public void Restart()
    {
        Time.timeScale = 1;
        losePanel.transform.parent.GetChild(10).gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Update the sumo counterValue and text
    public void decreaseSumo(GameObject theNPC)
    {
        sumoCounter = theNPC.gameObject.transform.parent.childCount;
        sumoCounterTX.text = sumoCounter.ToString();

        theNPC.GetComponent<NPCScript>().OutOfArea();

        //Calls win panel if there is no NPC
        if (isPlayerAlive && sumoCounter <= 1)
            WinTheRound();
    }

    //Increases the score value if any NPC throwed out
    public void IncreaseScore(int tempScore)
    {
        score += tempScore;
        scoreTX.text = score.ToString();
    }

    //Set timescale to 0 and set panels for pause screen
    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        winPanel.transform.GetChild(2).GetComponent<Text>().text = score.ToString();
        for (int i = 1; i <= 6; i++)
        {
            losePanel.transform.parent.GetChild(i).gameObject.SetActive(false);
        }
        losePanel.transform.parent.GetChild(10).gameObject.SetActive(false);
    }

    //Set timescale to 1 and set panels for play screen
    public void Continue()
    {
        for (int i = 1; i <= 6; i++)
        {
            losePanel.transform.parent.GetChild(i).gameObject.SetActive(true);
        }
        Time.timeScale = 1;
        losePanel.transform.parent.GetChild(10).gameObject.SetActive(true);
        losePanel.transform.parent.GetChild(11).gameObject.SetActive(false);
        pausePanel.SetActive(false);
    }

    //Set timescale to 0 and set panels for win screen
    public void WinTheRound()
    {
        for (int i = 1; i <= 6; i++)
        {
            losePanel.transform.parent.GetChild(i).gameObject.SetActive(false);
        }
        Time.timeScale = 0;
        winPanel.SetActive(true);
        winPanel.transform.GetChild(2).GetComponent<Text>().text = score.ToString();
        losePanel.transform.parent.GetChild(10).gameObject.SetActive(false);
    }

    //Set timescale to 0 and set panels for lose screen according to lose state
    public void GameOver(string overCase)
    {
        Time.timeScale = 0;
        losePanel.SetActive(true);
        losePanel.transform.GetChild(2).GetComponent<Text>().text = score.ToString();
        for (int i = 1; i <= 6; i++)
        {
            losePanel.transform.parent.GetChild(i).gameObject.SetActive(false);
        }
        losePanel.transform.parent.GetChild(10).gameObject.SetActive(false);

        //Show the lose panel message text according to lose state
        if (overCase == "down")
        {
            losePanel.transform.GetChild(0).GetComponent<Text>().text = "You FALL!";
        }
        else if(overCase == "time")
        {
            losePanel.transform.GetChild(0).GetComponent<Text>().text = "Time is up!";
        }
    }


}
