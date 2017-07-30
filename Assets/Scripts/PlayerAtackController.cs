using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtackController : MonoBehaviour {

    public GameObject bullet;
    public float bulletSpeed;
    PlayerStats stats;
    GameObject gm;

	void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        stats = gm.GetComponent<PlayerStats>();
    }
	
	void Update () {
        if (Input.GetButtonDown("Fire1") && !stats.droppingBeacon && !stats.isLowHealth())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject myBullet = Instantiate(bullet, this.transform.position, transform.rotation);
                Vector3 direction = (hit.point - myBullet.transform.position).normalized;
                direction.y = 0;
                myBullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            }

            stats.LoseHealth(7.5f);
        }

        if (Input.GetButtonDown("Fire2"))
        {

        }
    }
}
