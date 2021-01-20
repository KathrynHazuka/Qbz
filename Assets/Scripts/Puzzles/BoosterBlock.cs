using UnityEngine;
using System.Collections;

public class BoosterBlock : MonoBehaviour {

	public float xForce;
	public float zForce;

    void OnTriggerStay(Collider other)
    {
        //Push object on top of booster block forward
        if (other.GetComponent<Rigidbody>() != null)
            other.GetComponent<Rigidbody>().AddForce(xForce, 0, zForce);
        else if (other.GetComponentInParent<Rigidbody>() != null)
            other.GetComponentInParent<Rigidbody>().AddForce(xForce, 0, zForce);
    }
}
