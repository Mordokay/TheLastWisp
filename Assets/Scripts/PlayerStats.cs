using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public bool droppingBeacon = true;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (droppingBeacon)
            {
                droppingBeacon = false;
                player.GetComponent<ItemDropper>().CleanBeacons();
            }
            else
            {
                droppingBeacon = true;
            }
        }
    }
}
