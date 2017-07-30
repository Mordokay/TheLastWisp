using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour {

    GameObject gm;
    RaycastHit hit;
    Ray ray;
    GameObject temporaryBeaconGreen;
    GameObject temporaryBeaconRed;
    public GameObject beaconGreen;
    public GameObject beaconRed;
    public GameObject beaconNormal;

    void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager");
	}
	
    public void CleanBeacons()
    {
        Destroy(temporaryBeaconGreen);
        Destroy(temporaryBeaconRed);
    }

	void Update () {
        if (gm.GetComponent<PlayerStats>().droppingBeacon)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag.Equals("Ground"))
                {
                    if (temporaryBeaconRed)
                    {
                        temporaryBeaconRed.SetActive(false);
                    }

                    if (temporaryBeaconGreen == null)
                    {
                        temporaryBeaconGreen = Instantiate(beaconGreen) as GameObject;
                    }
                    else if (!temporaryBeaconGreen.activeSelf)
                    {
                        temporaryBeaconGreen.SetActive(true);
                    }

                    temporaryBeaconGreen.transform.position = hit.point;
                }
                else
                {
                    if (temporaryBeaconGreen)
                    {
                        temporaryBeaconGreen.SetActive(false);
                    }
                    if (temporaryBeaconRed == null)
                    {
                        temporaryBeaconRed = Instantiate(beaconRed) as GameObject;
                    }
                    else if (!temporaryBeaconRed.activeSelf)
                    {
                        temporaryBeaconRed.SetActive(true);
                    }

                    temporaryBeaconRed.transform.position = hit.point;
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    if (temporaryBeaconGreen && temporaryBeaconGreen.activeSelf)
                    {
                        GameObject myBeacon = Instantiate(beaconNormal) as GameObject;
                        myBeacon.transform.position = hit.point;

                        Destroy(temporaryBeaconGreen);
                        Destroy(temporaryBeaconRed);
                    }
                }
            }
        }
	}
}
