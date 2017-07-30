using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject explosionPS;

    // Use this for initialization
    void Start ()
    {
        Destroy(this.gameObject, 2f);	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    	
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Shadow")
        {

        }
        
        GameObject explosion = Instantiate(explosionPS, this.transform.position, Quaternion.identity) as GameObject;
        explosion.GetComponent<ParticleSystem>().Play();
        Destroy(explosion, 1f);
        Destroy(gameObject, 0f);
    }
}
