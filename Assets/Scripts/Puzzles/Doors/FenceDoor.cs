using UnityEngine;
using System.Collections;

public class FenceDoor : MonoBehaviour
{

    [SerializeField]
    private GameObject controller;
    [SerializeField]
    private Transform door;
    [SerializeField]
    private bool checkState;

    private Animation anim;

    private bool doorDown = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animation>();
        if (checkState)
        {
            controller.SendMessage("ReadState", doorDown);
        }
        SetInitialState();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.isPlaying == true)
        {
            controller.SendMessage("Hold", true);
        }
        else
        {
            controller.SendMessage("Hold", false);
        }
    }

    void SetInitialState()
    {
        if (doorDown)
        {
            door.localPosition = new Vector3(-0.16f, 0, 0);
        }
    }

    void TurnOnOff(string toBe)
    {
        if (toBe == "OFF")
        {
            doorDown = false;
            anim.Play("FenceDoorUp");
        }
        else if (toBe == "ON")
        {
            doorDown = true;
            anim.Play("FenceDoorDown");
        }
    }
}