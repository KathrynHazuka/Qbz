using UnityEngine;
using System.Collections;

public class LazerInput : MonoBehaviour
{

    //Components
    private Animation anim;

    //Lazer reciever values
    public bool isOn;

    public GameObject target;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animation>();
        anim["LazerInputOnAnim"].speed = 0;
        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            LazerOn();
        }
        else {
            LazerOff();
        }
    }

    void Hold(bool toHold)
    {
        return;
    }

    void OnOffSwitch(bool willBeOn)
    {
        if (willBeOn == true)
        {
            isOn = true;
        }
        else if (willBeOn == false)
        {
            isOn = false;
        }
    }

    void LazerOn()
    {
        anim["LazerInputOnAnim"].speed = Mathf.Lerp(anim["LazerInputOnAnim"].speed, 1f, 5 * Time.deltaTime);
        target.SendMessage("TurnOnOff", "ON");
    }

    void LazerOff()
    {
        anim["LazerInputOnAnim"].speed = Mathf.Lerp(anim["LazerInputOnAnim"].speed, 0f, 5 * Time.deltaTime);
        target.SendMessage("TurnOnOff", "OFF");
    }
}
