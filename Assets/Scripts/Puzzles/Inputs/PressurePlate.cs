using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    private GameObject[] targets;

    private bool pressurePlateState;

    // Use this for initialization
    void Start()
    {
        pressurePlateState = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Box" || col.tag == "Redirector" || col.tag == "Player")
        {
            pressurePlateState = true;
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Box" || col.tag == "Redirector" || col.tag == "Player")
        {
            pressurePlateState = false;
        }
    }

    void CheckState()
    {
        if (pressurePlateState == true)
        {
            for (int i = 0; i < targets.Length; i++)
                targets[i].SendMessage("TurnOnOff", "ON");

        } 
        else if (pressurePlateState == false)
        {
            for (int i = 0; i < targets.Length; i++)
                targets[i].SendMessage("TurnOnOff", "OFF");
        }
    }
}
