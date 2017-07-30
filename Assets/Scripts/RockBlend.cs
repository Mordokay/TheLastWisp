using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBlend : MonoBehaviour {

    public Material material1;
    public Material material2;
    public float duration;
    Renderer rend;
    public float regenRatio;

    [Range(0, 1)]
    public float blend = 0.0f;

    public bool CanCharge;
    //Max 100
    public float Chargelevel;

    public bool maxedGlow;

    GameObject player;
    GameObject gm;
    public float powerTransferRatio = 5.0f;
    public GameObject particleConsumingEnergy;
    public float timeSinceLastContaminationAttempt;
    GameObject myParticles;

    Vector3 originalScale;

    void Start()
    {
        myParticles = Instantiate(particleConsumingEnergy);
        myParticles.SetActive(false);
        myParticles.transform.parent = this.transform;
        originalScale = myParticles.transform.localScale;

        rend = GetComponent<Renderer>();
        rend.material = material1;
        regenRatio = 10.0f;
        player = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GameManager");
        timeSinceLastContaminationAttempt = Time.time;
    }

    void Update()
    {
        if (maxedGlow && (Time.time - timeSinceLastContaminationAttempt > 2.0f) && CanCharge)
        {
            timeSinceLastContaminationAttempt = Time.time;
            int chance = Random.Range(0, 100);
            //There is a 1 in 20 chance that every second another wock will be contaminated
            if(chance == 44)
            {
                Debug.Log("ContaminateRock!!!");
            }
        }
        if (CanCharge)
        {
            //If player can suck energy and is close to the rock ... Rock loses charge over time and player gains charge
            if ((this.transform.position - player.transform.position).magnitude < 3.0f && !gm.GetComponent<PlayerStats>().isHighHealth() && Chargelevel > 0.0f)
            {
                gm.GetComponent<PlayerStats>().GainHealth(Time.deltaTime * powerTransferRatio);
                Chargelevel -= Time.deltaTime * powerTransferRatio * 6.0f;

                //particle effects
                myParticles.SetActive(true);
                myParticles.transform.position = transform.position;
                Vector3 targetDir = player.transform.position - transform.position;
                myParticles.transform.rotation = Quaternion.LookRotation(targetDir);
                //myParticles.transform.Rotate(-Vector3.right * 90.0f);
                //myParticles.transform.Rotate(-Vector3.forward * 90.0f);
                myParticles.transform.localScale = new Vector3(originalScale.x,
                    originalScale.y, originalScale.z * (this.transform.position - player.transform.position).magnitude * 0.3f);
            }
            else
            {
                myParticles.SetActive(false);
                //If energy reaches zero the rock cant charge anymore
                if (Chargelevel <= 0.0f)
                {
                    CanCharge = false;
                    Chargelevel = 0.0f;
                }
                else if (Chargelevel >= 100)
                {
                    maxedGlow = true;
                    Chargelevel = 100.0f;
                }
                else
                {
                    Chargelevel += Time.deltaTime * 6.0f;
                }
            }
        }
        rend.material.Lerp(material1, material2, Chargelevel / 100);
    }
}
