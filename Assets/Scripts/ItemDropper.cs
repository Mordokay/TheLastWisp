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
    public GameObject beaconNormalUp2;
    public GameObject beaconNormalUp3;

    GameObject temporaryBarrierGreen;
    GameObject temporaryBarrierRed;

    public GameObject barrierGreen;
    public GameObject barrierRed;
    public GameObject barrierNormal;
    public GameObject barrierNormalUp2;
    public GameObject barrierNormalUp3;

    float barrierRotation = 0;
    public float rotationSpeed;

    void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager");
	}
	
    public void CleanBeacons()
    {
        Destroy(temporaryBeaconGreen);
        Destroy(temporaryBeaconRed);
    }
    public void CleanBarriers()
    {
        barrierRotation = 0.0f;
        Destroy(temporaryBarrierGreen);
        Destroy(temporaryBarrierRed);
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
                        GameObject myBeacon = new GameObject();
                        switch (gm.GetComponent<PlayerStats>().beaconLevel)
                        {
                            case 1:
                                myBeacon = Instantiate(beaconNormal) as GameObject;
                                break;
                            case 2:
                                myBeacon = Instantiate(beaconNormalUp2) as GameObject;
                                break;
                            case 3:
                                myBeacon = Instantiate(beaconNormalUp3) as GameObject;
                                break;
                        }
                        myBeacon.transform.position = hit.point;
                        gm.GetComponent<PlayerStats>().beacons.Add(myBeacon);
                        Destroy(temporaryBeaconGreen);
                        Destroy(temporaryBeaconRed);
                    }
                }
            }
        }

        if (gm.GetComponent<PlayerStats>().droppingBarrier)
        {
            barrierRotation += (Input.GetAxis("Mouse ScrollWheel") * rotationSpeed * Time.deltaTime);

            if (temporaryBarrierGreen != null)
            {
                temporaryBarrierGreen.transform.rotation = Quaternion.Euler(0, barrierRotation, 0);
            }
            if (temporaryBarrierRed != null)
            {
                temporaryBarrierRed.transform.rotation = Quaternion.Euler(0, barrierRotation, 0);
            }

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag.Equals("Ground"))
                {
                    if (temporaryBarrierRed)
                    {
                        temporaryBarrierRed.SetActive(false);
                    }

                    if (temporaryBarrierGreen == null)
                    {
                        temporaryBarrierGreen = Instantiate(barrierGreen) as GameObject;
                    }
                    else if (!temporaryBarrierGreen.activeSelf)
                    {
                        temporaryBarrierGreen.SetActive(true);
                    }

                    temporaryBarrierGreen.transform.position = hit.point;
                }
                else
                {
                    if (temporaryBarrierGreen)
                    {
                        temporaryBarrierGreen.SetActive(false);
                    }
                    if (temporaryBarrierRed == null)
                    {
                        temporaryBarrierRed = Instantiate(barrierRed) as GameObject;
                    }
                    else if (!temporaryBarrierRed.activeSelf)
                    {
                        temporaryBarrierRed.SetActive(true);
                    }

                    temporaryBarrierRed.transform.position = hit.point;
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    if (temporaryBarrierGreen && temporaryBarrierGreen.activeSelf)
                    {
                        GameObject myBarrier = new GameObject();
                        switch (gm.GetComponent<PlayerStats>().barrierLevel)
                        {
                            case 1:
                                myBarrier = Instantiate(barrierNormal) as GameObject;
                                break;
                            case 2:
                                myBarrier = Instantiate(barrierNormalUp2) as GameObject;
                                break;
                            case 3:
                                myBarrier = Instantiate(barrierNormalUp3) as GameObject;
                                break;
                        }
                        myBarrier.transform.position = hit.point;
                        myBarrier.transform.rotation = Quaternion.Euler(0, barrierRotation, 0);
                        Destroy(temporaryBarrierGreen);
                        Destroy(temporaryBarrierRed);
                    }
                }
            }
        }
    }
}
