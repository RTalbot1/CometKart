using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int numLaps = 3;
    public Text raceResultText;
    
    public int playerLaps = 0;
    private int botLaps = 0;
    private bool raceStarted = false;
    public bool raceFinished = false;
    private float raceTime = 0f;

    public GameObject[] characters;
    public Rigidbody sphere;
    public GameObject player1; 
    private GameObject player;
    private GameObject bot1;
    private GameObject bot2;
    private GameObject bot3;

    private GameObject aiPaths;

    //public GameObject camera;
    public CinemachineVirtualCamera virtualCamera;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
         for (int i = 0; i < walls.Length; i++) {
            MeshRenderer mr = walls[i].GetComponent<MeshRenderer>();
            mr.enabled = false;
        }

        
        for (int i = 0; i < characters.Length; i++) {
            characters[i].SetActive(false);
        }
        int char_int = PlayerPrefs.GetInt("SelectedCharacter", 0);
        player = characters[char_int];

        virtualCamera.Follow = player.transform;
        //virtualCamera.LookAt = player.transform;

      
        //save the player input
        PlayerInput pi = player1.GetComponent<PlayerInput>();
       

        // set up player
        setUpRigidBody(player);
        setUpBoxCollider(player);
        setUpBounce(player, sphere);
        setUpCarController(player, sphere, pi);
        
        player.tag = "Player";
        player.SetActive(true);
       
        for (int i = 0; i < characters.Length; i++) {
            if (characters[i] != player) {
                characters[i].SetActive(false);
            }
        }

        aiPaths = GameObject.FindGameObjectsWithTag("Paths").First();

        bot1 = player;
        bot2 = player;
        bot3 = player;
        int botIndex = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != player)
            {
                switch (botIndex)
                {
                    case 0:
                        bot1 = characters[i];
                        break;
                    case 1:
                        bot2 = characters[i];
                        break;
                    case 2:
                        bot3 = characters[i];
                        break;
                }
                botIndex++;
            }
        }

        SetUpBot(bot1, "Path1", new Vector3(10, 0, 0));
        SetUpBot(bot2, "Path2", new Vector3(-10, 0, 0));
        SetUpBot(bot3, "Path3", new Vector3(-20, 0, 0));
    }

    void Update()
    {
        if (raceStarted && !raceFinished)
        {
            raceTime += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Detect lap completion by the player and bot
        // if (other.gameObject == player && playerLaps < numLaps)
        // {
        //     playerLaps++;
        //     lapText.text = "Player Lap: " + playerLaps + "/" + numLaps;
        // }
        // else if (other.gameObject == bot && botLaps < numLaps)
        // {
        //     botLaps++;
        //     lapText.text = "Bot Lap: " + botLaps + "/" + numLaps;
        // }

        // Check if the race has finished
        if (playerLaps == numLaps)
        {
            EndRace();
        }
    }

    public void StartRace()
    {
        // Enable vehicle controls and start the race
        player.GetComponent<CarController>().enabled = true;
        bot1.GetComponent<CarAIController>().enabled = true;
        bot2.GetComponent<CarAIController>().enabled = true;
        bot3.GetComponent<CarAIController>().enabled = true;
        raceStarted = true;
    }
    
    private void SetUpBot(GameObject bot, string pathName, Vector3 translate)
    {
        GameObject botSphereObj = new GameObject();
        botSphereObj.transform.localScale = new Vector3(1 / 3, 1 / 3, 1 / 3);
        SphereCollider sphereCollider = botSphereObj.AddComponent<SphereCollider>();
        sphereCollider.radius = 0.45f;
        Rigidbody botSphere = botSphereObj.AddComponent<Rigidbody>();
        botSphere.mass = sphere.mass;
        botSphere.drag = sphere.drag;
        botSphere.angularDrag = sphere.angularDrag;

        bot.transform.Translate(translate);
        botSphere.transform.position = sphere.position - translate;
        setUpRigidBody(bot);
        setUpBoxCollider(bot);
        setUpBounce(bot, botSphere);
        setUpCarAIController(bot, botSphere, aiPaths.transform.Find(pathName));
        bot.SetActive(true);
    }

    private void setUpRigidBody(GameObject player) 
    {
        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.mass = 1;
        rb.drag = 2;
        rb.angularDrag = 0.05F;
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }


    private void setUpBoxCollider(GameObject player) 
    {
        BoxCollider bc= player.AddComponent<BoxCollider>();
        bc.isTrigger = false;
        PhysicMaterial frictionless = Resources.Load("Material/frictionless.mat", typeof(PhysicMaterial)) as PhysicMaterial;
        bc.material = frictionless;
        bc.center = new Vector3(0F,-0.2F,0F);
        bc.size = new Vector3(2.5F, 1F, 4.6F);
    }

    private void setUpCarController(GameObject player, Rigidbody sphere, PlayerInput pi)
    {
        CarController c = player.AddComponent<CarController>();
        c.playerInput = pi;
        c.sphere = sphere;

        c.forwardAccel = 40f;
        c.reverseAccel = 4f;
        c.maxSpeed = 300f;
        c.turnStrength = 50f;
        c.driftStrength = 100f;
        c.gravityForce = 10;
        c.dragOnGround = 3;
        c.currentSpeed = 0;
        
        c.whatIsGround = LayerMask.GetMask("Ground");
        c.groundRayLength = 3F;

        GameObject rayPoint = new GameObject("rayPoint");
        rayPoint.transform.parent = player.transform;
        rayPoint.transform.position = player.transform.position + new Vector3(0f, -0.7f, 0f);
        c.groundRayPoint = rayPoint.transform;

        GameObject rayPoint2 = new GameObject("rayPoint2");
        rayPoint2.transform.parent = player.transform;
        rayPoint2.transform.position = player.transform.position + new Vector3(0F, -0.7F, 1.605F);
        c.groundRayPoint2 = rayPoint2.transform;


        GameObject leftTrail = new GameObject();
        TrailRenderer tr = leftTrail.AddComponent<TrailRenderer>();
        leftTrail.transform.position = new Vector3(722F-1.07F,141-0.69F,-60-1.6F);
        tr.time = 0.3F;
        tr.minVertexDistance = 0.1F;
        tr.startColor = new Color(0,214,214,1);
        tr.endColor = new Color(255,255,255,1);
        c.leftfront = leftTrail.transform;

        GameObject rightTrail = new GameObject();
        TrailRenderer trr = rightTrail.AddComponent<TrailRenderer>();
        rightTrail.transform.position = new Vector3(722F,141-0.69F,-60-1.6F);
        trr.time = 0.3F;
        trr.minVertexDistance = 0.1F;
        trr.startColor = new Color(0,214,214,1);
        trr.endColor = new Color(255,255,255,1);
        c.rightfront = rightTrail.transform;

        c.maxWheelTurn = 25;
        
        c.enabled = false;

    }

    private void setUpCarAIController(GameObject player, Rigidbody sphere, Transform path)
    {
        Debug.Log("Set up Car AI controller");
        CarAIController c = player.AddComponent<CarAIController>();
        c.sphere = sphere;

        c.forwardAccel = 40f;
        c.reverseAccel = 4f;
        c.maxSpeed = 300f;
        c.turnStrength = 300f;
        c.gravityForce = 10;
        c.dragOnGround = 3;
        c.currentSpeed = 0;

        c.whatIsGround = LayerMask.GetMask("Ground");
        c.groundRayLength = 3F;

        c.waypoints = new List<Transform>();

        foreach(Transform t in path)
        {
            c.waypoints.Add(t);
        }

        GameObject rayPoint = new GameObject("rayPoint");
        rayPoint.transform.parent = player.transform;
        rayPoint.transform.position = player.transform.position + new Vector3(0f, -0.7f, 0f);
        c.groundRayPoint = rayPoint.transform;

        GameObject rayPoint2 = new GameObject("rayPoint2");
        rayPoint2.transform.parent = player.transform;
        rayPoint2.transform.position = player.transform.position + new Vector3(0F, -0.7F, 1.605F);
        c.groundRayPoint2 = rayPoint2.transform;


        GameObject leftTrail = new GameObject();
        TrailRenderer tr = leftTrail.AddComponent<TrailRenderer>();
        leftTrail.transform.position = new Vector3(722F - 1.07F, 141 - 0.69F, -60 - 1.6F);
        tr.time = 0.3F;
        tr.minVertexDistance = 0.1F;
        tr.startColor = new Color(0, 214, 214, 1);
        tr.endColor = new Color(255, 255, 255, 1);
        c.leftfront = leftTrail.transform;

        GameObject rightTrail = new GameObject();
        TrailRenderer trr = rightTrail.AddComponent<TrailRenderer>();
        rightTrail.transform.position = new Vector3(722F, 141 - 0.69F, -60 - 1.6F);
        trr.time = 0.3F;
        trr.minVertexDistance = 0.1F;
        trr.startColor = new Color(0, 214, 214, 1);
        trr.endColor = new Color(255, 255, 255, 1);
        c.rightfront = rightTrail.transform;

        c.maxWheelTurn = 30;
        
        c.enabled = false;

    }
    private void setUpBounce(GameObject player, Rigidbody sphere)
    {
        Bounce b = player.AddComponent<Bounce>();
        b.player = player.GetComponent<Rigidbody>();
        b.sphere = sphere;

    }

    void EndRace()
    {
        // Calculate and display the race results
        raceFinished = true;
        //player.GetComponent<PlayerController>().enabled = false;
        //bot.GetComponent<BotController>().enabled = false;
        raceResultText.text = "Race Finished in " + raceTime.ToString("F2") + " seconds. ";
        
        if (playerLaps > botLaps)
        {
            raceResultText.text += "Player Wins!";
        }
        else if (botLaps > playerLaps)
        {
            raceResultText.text += "Bot Wins!";
        }
        else
        {
            raceResultText.text += "It's a Tie!";
        }
    }
}
