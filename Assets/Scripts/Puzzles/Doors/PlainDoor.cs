using UnityEngine;
using System.Collections;

public class PlainDoor : MonoBehaviour
{

    public GameObject controller;
    [SerializeField]
    private bool switchControl = false;

    private Animation anim;

    private bool doorDown = false;
    public string doorState = "";
    private Transform door;

    void Awake()
    {
        anim = GetComponent<Animation>();
    }

    // Use this for initialization
    void Start()
    {
        door = transform.Find("door");

        if (switchControl)
            controller.SendMessage("ReadState", doorDown);
    }

    void TurnOnOff(string toBe)
    {
        if (toBe == "OFF" && doorDown)
        {
            doorDown = false;
            anim.Play("PlainDoorUp");
        }
        else if (toBe == "ON" && !doorDown)
        {
            doorDown = true;
            anim.Play("PlainDoorDown");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (switchControl)
        {
            HoldController();
        }
    }

    void HoldController()
    {
        if (anim.isPlaying == true)
        {
            controller.SendMessage("Hold", true);
        }
        else {
            controller.SendMessage("Hold", false);
        }
        if (!anim.isPlaying)
        {
            SetDoorState();
        }
    }

    void SetDoorState()
    {
        if (door.localPosition.y == 1)
        {
            doorState = "Door is Up";
        }
        else
        if (door.localPosition.y == 0)
        {
            doorState = "Door is Down";
        }
    }
}
