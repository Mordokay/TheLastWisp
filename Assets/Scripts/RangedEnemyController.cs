﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyController : MonoBehaviour
{

    GameObject player;
    Vector3 previousTargetPosition;
    public GameObject energyParticles;
    public float minDistanceToShoot;
    public GameObject projectile;
    public float bulletSpeed;
    float timeSinceLastShoot;
    public float shootTimeInterval;
    GameObject gm;

    public float xpGain;
    public int life;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        life = 100;
        previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);
        player = GameObject.FindGameObjectWithTag("Player");
        this.GetComponent<NavMeshAgent>().destination = player.transform.position;
        InvokeRepeating("FollowTarget", 0.0f, 0.2f);
        this.transform.localPosition = new Vector3(this.transform.position.x, player.transform.position.y, this.transform.position.z);
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            Instantiate(energyParticles, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bullet"))
        {
            life -= 50;
            gm.GetComponent<PlayerStats>().playerXP += xpGain;
        }
        else if (other.gameObject.tag.Equals("LightSaber")){
            life -= 40;
            gm.GetComponent<PlayerStats>().playerXP += xpGain / 5;
        }

        if (life <= 0)
        {
            Instantiate(energyParticles, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            gm.GetComponent<PlayerStats>().enemyCount -= 1;
            gm.GetComponent<PlayerStats>().enemiesKilled += 1;
        }
    }

    void CheckCanShoot(GameObject target)
    {
        if (Time.time - timeSinceLastShoot > shootTimeInterval && 
            (this.transform.position - target.transform.position).magnitude < minDistanceToShoot)
        {
            GameObject myProjectile = Instantiate(projectile, this.transform.position, transform.rotation);
            Vector3 direction = (target.transform.position - this.transform.position).normalized;
            direction.y = 0;
            myProjectile.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            timeSinceLastShoot = Time.time;
        }
    }
    GameObject getCloserTarget() {
        GameObject mytarget = player;
        gm.GetComponent<PlayerStats>().beacons = gm.GetComponent<PlayerStats>().beacons.Where(item => item != null).ToList();
        foreach (GameObject beacon in gm.GetComponent<PlayerStats>().beacons)
        {
            if(Vector3.Distance(this.transform.position, mytarget.transform.position) - 2.0f > Vector3.Distance(this.transform.position, beacon.transform.position))
            {
                mytarget = beacon;
            }
        }

        return mytarget;
    }
    private void FollowTarget()
    {
        GameObject myTarget = getCloserTarget();
        if ((this.transform.position - myTarget.transform.position).magnitude > 4.0f)
        {
            // did target move more than at least a minimum amount since last destination set?
            if ((previousTargetPosition - myTarget.transform.position).magnitude > 0.1f)
            {
                this.GetComponent<NavMeshAgent>().SetDestination(myTarget.transform.position);
                previousTargetPosition = myTarget.transform.position;
            }
        }
        CheckCanShoot(myTarget);
    }
}
