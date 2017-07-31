using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {

    public GameObject explosionPS;

    void Start()
    {
        Destroy(this.gameObject, 2f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Energy")
        {
            GameObject explosion = Instantiate(explosionPS, this.transform.position, Quaternion.identity) as GameObject;
            explosion.GetComponent<ParticleSystem>().Play();
            Destroy(explosion, 1f);
            Destroy(gameObject, 0f);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Obstacle"){
            GameObject explosion = Instantiate(explosionPS, this.transform.position, Quaternion.identity) as GameObject;
            explosion.GetComponent<ParticleSystem>().Play();
            Destroy(explosion, 1f);
            Destroy(gameObject, 0f);
        }
    }
    
}
