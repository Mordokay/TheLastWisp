using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 2.0f;
    public float maxVelocity = 2.0f;

    void Start () {
		
	}
	
	void Update () {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction -= Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        this.GetComponent<Rigidbody>().AddForce(direction * speed);
        this.GetComponent<Rigidbody>().velocity = 
            Vector3.ClampMagnitude(this.GetComponent<Rigidbody>().velocity, maxVelocity);
    }
}
