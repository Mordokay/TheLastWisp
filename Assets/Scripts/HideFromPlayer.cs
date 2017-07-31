using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideFromPlayer : MonoBehaviour {

	Renderer rend;
    GameObject player;
    GameObject gm;
    void Start () {
        rend = GetComponent<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GameManager");
    }
	
	// Update is called once per frame
	void Update () {
        if ((this.transform.position - player.transform.position).magnitude > ((gm.GetComponent<PlayerStats>().finalHealth * 7) / 110))
        {
            rend.material.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            rend.material.SetColor("_EmissionColor", Color.white);
        }
    }
}
