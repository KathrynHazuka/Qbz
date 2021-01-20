using UnityEngine;
using System.Collections;

public class MovableBox : MonoBehaviour
{
    //Reference to player script
    private Player playerScript;

    //Set initial spawn position 
    private Vector3 spawnPos;

    //State variable
    public string boxState = "Down";

    //Collision testing raycast hits
    private RaycastHit hitUp;
    private RaycastHit hitDown;
    private RaycastHit hitRight;
    private RaycastHit hitLeft;

    private bool playerUp;
    private bool playerDown;
    private bool playerRight;
    private bool playerLeft;

    private Prompt prompt;
    private Renderer _renderer;

    [SerializeField]
    private Material normalMat;
    [SerializeField]
    private Material glowMat;

    void Awake()
    {
        prompt = GetComponentInChildren<Prompt>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    // Use this for initialization
    void Start()
    {
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        while (playerScript == null && GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Player>() != null)
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Player>();
        }

        FallingOffCheck();
        CollisionTest();
        Prompt();
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

    ////Display prompt to turn linked object ON and OFF
    //void OnTriggerStay(Collider col)
    //{
    //    if (col.tag == "Player")
    //    {
    //        //Create string for prompt
    //        if (boxState == "Down" && !col.GetComponent<Player>().grabbing)
    //        {
    //            loadPrompt = "[E] to Grab";
    //        }
    //        else
    //        {
    //            loadPrompt = "[E] to Drop";
    //        }
    //    }
    //}

    void CollisionTest()
    {
        if (Physics.Raycast(transform.position, new Vector3(0, 0, -1), out hitUp, 0.55f))
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

        if (Physics.Raycast(transform.position, new Vector3(0, 0, 1), out hitDown, 0.55f))
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

        if (Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out hitRight, 0.55f))
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

        if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), out hitLeft, 0.55f))
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
        if (boxState == "Down" && Input.GetButtonDown("Action1"))
        {
            transform.position = playerHit.transform.position + direction;

            gameObject.AddComponent<FixedJoint>();
            gameObject.GetComponent<FixedJoint>().connectedBody = playerHit.rigidbody;
            playerScript.grabbing = true;
            playerScript.grabbingTransform = this.transform;
            boxState = "Up";
        }
        else if (boxState == "Up" && Input.GetButtonDown("Action1"))
        {
            Destroy(gameObject.GetComponent<FixedJoint>());
            playerScript.grabbing = false;
            boxState = "Down";
        }
    }

    void Prompt()
    {
        if (!playerUp && !playerDown && !playerRight && !playerLeft)
        {
            prompt.Disable();
            _renderer.material = normalMat;
        }

        if (playerUp || playerDown || playerRight || playerLeft)
        {
            _renderer.material = glowMat;
            //Create string for prompt
            if (boxState == "Down")
            {
                prompt.Enable("MoveUp");
            }
            else
            {
                prompt.Enable("MoveDown");
            }
        }
    }
}
