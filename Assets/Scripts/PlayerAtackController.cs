using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAtackController : MonoBehaviour {

    public GameObject bullet;
    public GameObject lightSaber;
    public float bulletSpeed;
    public float lightSaberEnergy;
    public float energyConsumptionRate;
    public Image lightSaberBar;
    bool up;

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
        if (Input.GetButtonDown("Fire2") && !stats.droppingBeacon && lightSaberEnergy > 0)
        {
            lightSaber.gameObject.SetActive(true);
            int rand = Random.Range(0, 2);
            if(rand == 1)
            {
                up = true;
            }
            else { up = false; }

            lightSaberEnergy -= Time.deltaTime * energyConsumptionRate;
            lightSaberBar.fillAmount = lightSaberEnergy / 100;
        }

        if (Input.GetButton("Fire2") && !stats.droppingBeacon && lightSaberEnergy > 0)
        {
            if (up)
            {
                lightSaber.transform.RotateAround(this.gameObject.transform.position, Vector3.up, 1500 * Time.deltaTime);
            }
            else { lightSaber.transform.RotateAround(this.gameObject.transform.position, Vector3.down, 1500 * Time.deltaTime); }

            lightSaberEnergy -= Time.deltaTime * energyConsumptionRate;
            lightSaberBar.fillAmount = lightSaberEnergy / 100;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            lightSaber.gameObject.SetActive(false);
        }
    }
}
