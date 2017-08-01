using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {

    public GameObject explosionPS;
    GameObject gm;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        gm.GetComponent<PlayerStats>().InstantiateBulletSound();
        Destroy(this.gameObject, 2f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Energy" && other.gameObject.tag != "RangedEnemy")
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                gm.GetComponent<PlayerStats>().LoseHealth(10.0f);
            }

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
