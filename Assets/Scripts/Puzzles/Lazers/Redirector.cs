using UnityEngine;
using System.Collections;

public class Redirector : MonoBehaviour
{

    private float timeToTurnOn = 0.25f;

    private string currentColTag = "";
    private string prevColTag = "";

    private float currentRedirectorID;
    private float prevRedirectorID;
    private string readInitialState = "";

    private Redirector currentRedirectorScript;
    private Redirector prevRedirectorScript;

    private GameObject player;
    private Player playerScript;
    private LazerInput lazerInputScript;

    private RaycastHit hit;
    private LineRenderer lr;
    private Animation anim;

    private int smooth = 15;
    public bool lazerOn = false;
    public int lazerCount = 0;

    //Set initial spawn position 
    private Vector3 spawnPos;

    //GUI variables
    public GUISkin promptUISkin;
    private string loadPrompt = "";

    //State variable
    public string moveState = "Down";

    //Collision testing raycast hits
    private RaycastHit hitUp;
    private RaycastHit hitDown;
    private RaycastHit hitRight;
    private RaycastHit hitLeft;

    private bool playerUp;
    private bool playerDown;
    private bool playerRight;
    private bool playerLeft;

    public float redirectorID;

    [SerializeField]
    private Prompt movePrompt;
    [SerializeField]
    private Prompt rotatePrompt;

    [SerializeField]
    private Renderer _frameRenderer;
    [SerializeField]
    private Material normalMat;
    [SerializeField]
    private Material glowMat;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        anim = GetComponent<Animation>();
        anim["RedirectorBallAnim"].speed = 0;

        redirectorID = Random.Range(0f, 10000000000f);
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        anim.Play();

        while (playerScript == null)
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Player>();
        }

        CollisionTest();
        FallingOffCheck();
        ResetPrompt();

        if (lazerCount > 0)
        {
            anim["RedirectorBallAnim"].speed = Mathf.Lerp(anim["RedirectorBallAnim"].speed, 1f, smooth * Time.deltaTime);
            LazerOn();
        }
        else
        {
            LazerOff();
        }
    }

    //recieves command to turn lazer ON and OFF
    void IncreaseLazerCount(bool on)
    {
        if (on)
        {
            lazerCount += 1;
        }
        else
        {
            lazerCount -= 1;
        }
    }

    //Called when lazer hits a collider
    void LazerCollision()
    {
        lr.SetPosition(1, new Vector3(0, 0, -hit.distance));

        if (currentColTag != prevColTag || currentRedirectorID != prevRedirectorID)
        {
            //If lazer hits player, tell player they are in a lazer
            if (currentColTag == "Player")
            {
                playerScript.inLazerCount += 1;
            }
            //If lazer hits the lazer input, turn it on
            else if (currentColTag == "LazerInput" && lr.enabled == true)
            {
                lazerInputScript = hit.transform.parent.GetComponent<LazerInput>();
                lazerInputScript.SendMessage("OnOffSwitch", true);
            }
            //If lazer hits the lazer redirector
            else if (currentColTag == "Redirector" && lr.enabled == true)
            {
                currentRedirectorScript = hit.transform.GetComponent<Redirector>();
                currentRedirectorScript.SendMessage("IncreaseLazerCount", true);
            }
        }
        ResetValues();
    }

    //Called when lazer hits nothing
    void LazerNoCollision()
    {
        lr.SetPosition(1, new Vector3(0, 0, -50));
    }

    void ResetValues()
    {
        if (prevColTag != currentColTag || prevRedirectorID != currentRedirectorID)
        {
            if (prevColTag == "Player")
            {
                playerScript.inLazerCount -= 1;
            }
            else if (prevColTag == "LazerInput")
            {
                lazerInputScript.SendMessage("OnOffSwitch", false);
            }
            else if (prevColTag == "Redirector")
            {
                readInitialState = "";
                prevRedirectorScript.SendMessage("IncreaseLazerCount", false);
            }
        }
    }

    void LazerOn()
    {
        lr.enabled = true;
        if (Physics.Raycast(transform.position, -transform.forward, out hit))
        {
            //TODO: Add specific tags for lazer passible tags
            if (hit.collider.tag != "LazerPass")
            {
                currentColTag = hit.collider.tag;
                if (currentColTag == "Redirector")
                {
                    currentRedirectorID = hit.collider.GetComponentInParent<Redirector>().redirectorID;
                    
                    if (readInitialState == "")
                    {
                        readInitialState = hit.collider.GetComponentInParent<Redirector>().lazerOn.ToString();
                    }

                    if (readInitialState == "False")
                    {
                       LazerCollision();
                    }
                    else
                    {
                        currentColTag = "";
                        readInitialState = "";
                        ResetValues();
                        LazerNoCollision();
                    }
                }
                else
                {
                    LazerCollision();
                }
            }
        }
        else
        {
            currentColTag = "";
            readInitialState = "";
            ResetValues();
            LazerNoCollision();
        }
        //Set previous collider tag to previously hit object
        prevColTag = currentColTag;
        prevRedirectorID = currentRedirectorID;
        prevRedirectorScript = currentRedirectorScript;
    }

    void LazerOff()
    {
        lr.enabled = false;
        anim["RedirectorBallAnim"].speed = Mathf.Lerp(anim["RedirectorBallAnim"].speed, 0f, 0.008234f / timeToTurnOn);
        currentColTag = "";
        ResetValues();
        prevColTag = "";
    }

    void FallingOffCheck()
    {
        //Check if Player is falling off the map
        if (transform.position.y < 0.2)
        {
            StartCoroutine(RespawnBox(.5f));
        }
    }

    //Respawn box after a certein amount of time
    IEnumerator RespawnBox(float respawnTimer)
    {
        //Move box out of visible scene
        transform.position = new Vector3(0, 100, 0);
        //Wait for respawn time
        yield return new WaitForSeconds(respawnTimer);
        //Respawn box in spawn position
        transform.position = spawnPos;
    }

    void CollisionTest()
    {
        if (Physics.Raycast(transform.position, new Vector3(0, 0, -1), out hitUp, 0.60f))
        {
            if (hitUp.collider.tag == "Player")
            {
                playerUp = true;
                GetGrabbed(hitUp, new Vector3(0, 0, 1));
            }
            else
            {
                playerUp = false;
            }
        }
        else
        {
            playerUp = false;
        }

        if (Physics.Raycast(transform.position, new Vector3(0, 0, 1), out hitDown, 0.60f))
        {
            if (hitDown.collider.tag == "Player")
            {
                playerDown = true;
                GetGrabbed(hitDown, new Vector3(0, 0, -1));
            }
            else
            {
                playerDown = false;
            }
        }
        else
        {
            playerDown = false;
        }

        if (Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out hitRight, 0.60f))
        {
            if (hitRight.collider.tag == "Player")
            {
                playerRight = true;
                GetGrabbed(hitRight, new Vector3(1, 0, 0));
            }
            else
            {
                playerRight = false;
            }
        }
        else
        {
            playerRight = false;
        }

        if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), out hitLeft, 0.60f))
        {
            if (hitLeft.collider.tag == "Player")
            {
                playerLeft = true;
                GetGrabbed(hitLeft, new Vector3(-1, 0, 0));
            }
            else
            {
                playerLeft = false;
            }
        }
        else
        {
            playerLeft = false;
        }
    }

    void GetGrabbed(RaycastHit playerHit, Vector3 direction)
    {
        if (moveState == "Down" && Input.GetButtonDown("Action1"))
        {
            transform.position = playerHit.transform.position + direction;
            gameObject.AddComponent<FixedJoint>();
            gameObject.GetComponent<FixedJoint>().connectedBody = playerHit.rigidbody;
            playerScript.grabbing = true;
            playerScript.grabbingTransform = this.transform;
            moveState = "Up";
        }
        else if (moveState == "Up" && Input.GetButtonDown("Action1"))
        {
            Destroy(gameObject.GetComponent<FixedJoint>());
            playerScript.grabbing = false;
            moveState = "Down";
        }
        if (Input.GetButtonDown("Action2"))
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 90f, 0);
            movePrompt.Rotate();
            rotatePrompt.Rotate();
        }
    }

    void ResetPrompt()
    {
        if (!playerUp && !playerDown && !playerRight && !playerLeft)
        {
            movePrompt.Disable();
            rotatePrompt.Disable();
            _frameRenderer.material = normalMat;
        }

        if (playerUp || playerDown || playerRight || playerLeft)
        {
            rotatePrompt.Enable("Rotate");
            _frameRenderer.material = glowMat;

            if (moveState == "Down" && !playerScript.grabbing)
            {
                movePrompt.Enable("MoveUp");
            }
            else
            {
                movePrompt.Enable("MoveDown");
            }
        }
    }
}
