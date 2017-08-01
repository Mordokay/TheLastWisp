using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAtackController : MonoBehaviour {

    public GameObject bullet;
    public GameObject lightSaber;
    public float bulletSpeed;
    public float energyConsumptionRate;
    bool up;

    PlayerStats stats;
    GameObject gm;
    public GameObject shootAudio;
    public GameObject lightsaberAudio;
    public float timeSinceLastSaber;

    void Start () {
        timeSinceLastSaber = 0.0f;
        gm = GameObject.FindGameObjectWithTag("GameManager");
        stats = gm.GetComponent<PlayerStats>();
    }
	
	void Update () {
        timeSinceLastSaber += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && !stats.droppingBeacon && !stats.isLowHealth() && !stats.droppingBarrier && !Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject myBullet = Instantiate(bullet, this.transform.position, transform.rotation);
                Vector3 direction = (hit.point - myBullet.transform.position).normalized;
                direction.y = 0;
                myBullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
                stats.LoseHealth(7.5f);

                GameObject myShoot = Instantiate(shootAudio);
                myShoot.GetComponent<AudioSource>().Play();
                Destroy(myShoot, 2.0f);
            }            
        }
        if (Input.GetButtonDown("Fire2") && !stats.droppingBeacon && !stats.isLowHealth() && !stats.droppingBarrier && !Input.GetButtonDown("Fire1"))
        {
            lightSaber.gameObject.SetActive(true);
            int rand = Random.Range(0, 2);
            if(rand == 1)
            {
                up = true;
            }
            else { up = false; }

            stats.LoseHealth(Time.deltaTime * energyConsumptionRate / 10);
        }

        if (Input.GetButton("Fire2") && !stats.droppingBeacon && !stats.isLowHealth() && !stats.droppingBarrier && !Input.GetButtonDown("Fire1"))
        {
            if (timeSinceLastSaber > 0.2f)
            {
                timeSinceLastSaber = 0.0f;
                GameObject myLightsaber = Instantiate(lightsaberAudio);
                myLightsaber.GetComponent<AudioSource>().Play();
                Destroy(myLightsaber, 1.0f);
            }
            if (up)
            {
                lightSaber.transform.RotateAround(this.gameObject.transform.position, Vector3.up, 1500 * Time.deltaTime);
            }
            else { lightSaber.transform.RotateAround(this.gameObject.transform.position, Vector3.down, 1500 * Time.deltaTime); }

            stats.LoseHealth(Time.deltaTime * energyConsumptionRate);
        }

        if (Input.GetButtonUp("Fire2"))
        {
            lightSaber.gameObject.SetActive(false);
        }
    }
}
