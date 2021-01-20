using UnityEngine;
using System.Collections;

public class LazerOutput : MonoBehaviour
{

    private float timeToTurnOn = 0.2f;

    private string currentColTag = "";
    private string prevColTag = "";

    private float currentRedirectorID;
    private float prevRedirectorID;

    private Redirector currentRedirectorScript;
    private Redirector prevRedirectorScript;

    private GameObject player;
    private Player playerScript;
    private LazerInput lazerInputScript;

    private float dieTimer;

    private RaycastHit hit;
    private LineRenderer lr;
    private Animation anim;

    public bool lazerOn = false;
    public bool stateCheck = false;
    public GameObject controller;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        anim = GetComponent<Animation>();
        anim["LazerRingRotationAnim"].speed = 0;
        if (stateCheck)
        {
            controller.SendMessage("ReadState", lazerOn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.Play();

        while (playerScript == null)
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Player>();
        }

        //If lazer is on, test for any objects in the way
        if (lazerOn)
        {
            anim["LazerRingRotationAnim"].speed = Mathf.Lerp(anim["LazerRingRotationAnim"].speed, 1f, Time.deltaTime / timeToTurnOn);
            if (anim["LazerRingRotationAnim"].speed >= 0.99f)
            {
                LazerOn();
            }
        }
        else {
            LazerOff();
        }
    }

    //recieves command to turn lazer ON and OFF
    void TurnOnOff(string toBe)
    {
        if (toBe == "ON")
        {
            lazerOn = true;
        }
        else if (toBe == "OFF")
        {
            lazerOn = false;
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
                }
                LazerCollision();
            }
        }
        else
        {
            currentColTag = "";
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
        anim["LazerRingRotationAnim"].speed = Mathf.Lerp(anim["LazerRingRotationAnim"].speed, 0f, Time.deltaTime / timeToTurnOn);
        currentColTag = "";
        ResetValues();
        prevColTag = "";
    }
}