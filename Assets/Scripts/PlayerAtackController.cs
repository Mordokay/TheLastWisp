using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtackController : MonoBehaviour {

    public GameObject bullet;
    public float bulletSpeed;
    GameObject gm;

	void Start () {
        bulletSpeed = 10.0f;
        gm = GameObject.FindGameObjectWithTag("GameManager");
    }
	
	void Update () {
        if (Input.GetButtonDown("Fire1") && !gm.GetComponent<PlayerStats>().droppingBeacon)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject myBullet = Instantiate(bullet, this.transform.position, transform.rotation);
                Vector3 direction = hit.point - myBullet.transform.position;
                direction.y = 0;
                myBullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            }
        }
    }
}
