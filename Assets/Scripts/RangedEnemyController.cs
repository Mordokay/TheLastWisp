using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);
        player = GameObject.FindGameObjectWithTag("Player");
        this.GetComponent<NavMeshAgent>().destination = player.transform.position;
        InvokeRepeating("FollowTarget", 0.0f, 0.1f);
        this.transform.localPosition = new Vector3(this.transform.position.x, player.transform.position.y, this.transform.position.z);
        Debug.Log(this.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            Instantiate(energyParticles, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    void CheckCanShoot()
    {
        if (Time.time - timeSinceLastShoot > shootTimeInterval && 
            (this.transform.position - player.transform.position).magnitude < minDistanceToShoot)
        {
            GameObject myProjectile = Instantiate(projectile, this.transform.position, transform.rotation);
            Vector3 direction = (player.transform.position - this.transform.position).normalized;
            direction.y = 0;
            myProjectile.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            timeSinceLastShoot = Time.time;
        }
    }

    private void FollowTarget()
    {
        if ((this.transform.position - player.transform.position).magnitude > 3.0f)
        {

            // did target move more than at least a minimum amount since last destination set?
            if ((previousTargetPosition - player.transform.position).magnitude > 0.1f)
            {
                this.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                previousTargetPosition = player.transform.position;
            }
        }
        CheckCanShoot();
    }
}
