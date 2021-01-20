using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour
{

    //Target game object for the button to control
    public Transform target;

    //Get components
    private Animation anim;

    //Current state of button
    private string buttonState;
    private Transform handle;
    private bool hold;

    //String to display prompts
    private GUISkin promptUISkin;
    private string loadPrompt;
    private bool inRange = false;

    private Prompt prompt;

    [SerializeField]
    private Renderer _blockRenderer;
    [SerializeField]
    private Material normalMat;
    [SerializeField]
    private Material glowMat;

    void Awake()
    {
        prompt = GetComponentInChildren<Prompt>();
    }

    void Start()
    {
        anim = GetComponent<Animation>();
        handle = transform.Find("handle");
    }

    void ReadState(bool state)
    {
        Start();
        if (state == true)
        {
            buttonState = "ON";
            handle.transform.localPosition = new Vector3(0f, .225f, -.375f);
        }
        else if (state == false)
        {
            buttonState = "OFF";
            handle.transform.localPosition = new Vector3(0f, -.225f, -.375f);
        }
    }

    void Update()
    {
        SwitchComm();
        Prompt();
    }

    void SwitchComm()
    {
        if (hold == false)
        {
            if (inRange && buttonState == "OFF" && Input.GetButtonDown("Action1"))
            {
                target.SendMessage("TurnOnOff", "ON");
                buttonState = "ON";
                anim.Play("SwitchUpAnim");
            }
            else if (inRange && buttonState == "ON" && Input.GetButtonDown("Action1"))
            {
                target.SendMessage("TurnOnOff", "OFF");
                buttonState = "OFF";
                anim.Play("SwitchDownAnim");
            }
        }
    }

    void Hold(bool toHold)
    {
        if (toHold == true)
        {
            hold = true;
        }
        else if (toHold == false)
        {
            hold = false;
        }
    }

    //Display prompt to turn linked object oON and OFF
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            inRange = true;
        }
    }

    //Delete string for prompts when player moves away
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            inRange = false;
        }
    }

    void Prompt()
    {
        if (inRange)
        {
            _blockRenderer.material = glowMat;

            if (buttonState == "OFF")
                prompt.Enable("PowerOn");
            else
                prompt.Enable("PowerOff");
        }
        else
        {
            _blockRenderer.material = normalMat;
            prompt.Disable();
        }
    }
}
