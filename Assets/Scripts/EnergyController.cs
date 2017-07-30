using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour {

    bool goAfterPlayer = false;
    Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        if((this.transform.position - player.position).magnitude < 0.2f)
        {
            //Destroyes particle and gives player more energy
            Destroy(this.gameObject);
        }
    }
}
