using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour {

    bool goAfterPlayer = false;
    Transform player;
    GameObject gm;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gm = GameObject.FindGameObjectWithTag("GameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!goAfterPlayer && other.gameObject.tag.Equals("Player"))
        {
            goAfterPlayer = true;
        }
    }

    private void Update()
    {
        if (goAfterPlayer)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, player.position, Time.deltaTime * 5.0f);
        }
        if((this.transform.position - player.position).magnitude < 0.2f && !gm.GetComponent<PlayerStats>().isHighHealth())
        {
            //Destroyes particle and gives player more energy
            Destroy(this.gameObject);
            gm.GetComponent<PlayerStats>().GainHealth(10.0f);
        }
    }
}
