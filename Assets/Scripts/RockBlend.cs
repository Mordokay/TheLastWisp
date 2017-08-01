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
    public float powerTransferRatio;
    public GameObject particleConsumingEnergy;
    public float timeSinceLastContaminationAttempt;
    GameObject myParticles;

    Vector3 originalScale;
    public GameObject map;

    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map");
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

    void ContaminateRock()
    {
        int xPos = (int)transform.position.x + (int)(map.GetComponent<MapTerrainGenerator>().sizeX / 2);
        int yPos = (int)transform.position.z + (int)(map.GetComponent<MapTerrainGenerator>().sizeY / 2);

        //Debug.Log("ContaminateRock!!!  -> " + new Vector2(this.transform.position.x, this.transform.position.z));
       // Debug.Log("Converted!!!  -> " + new Vector2(xPos, yPos));

        for (int i = xPos - 1; i <= xPos + 1; i++)
        {
            for (int j = yPos - 1; j <= yPos + 1; j++)
            {
                if(i >= 0 && j >= 0 && i < map.GetComponent<MapTerrainGenerator>().sizeX && j < map.GetComponent<MapTerrainGenerator>().sizeY)
                {
                    if(!(i == xPos && j == yPos) && gm.GetComponent<PlayerStats>().rocks[i, j] == 2 && gm.GetComponent<PlayerStats>().rockGlowCount < gm.GetComponent<PlayerStats>().maxGlowRocks)
                    {
                        gm.GetComponent<PlayerStats>().rockObjects[i, j].GetComponent<RockBlend>().CanCharge = true;
                        gm.GetComponent<PlayerStats>().rockObjects[i, j].GetComponent<RockBlend>().Chargelevel = 1.0f;
                        gm.GetComponent<PlayerStats>().rockGlowCount += 1;
                        //Debug.Log("Charge on: " + i + " <> " + j + "  With rock glow count at: " + gm.GetComponent<PlayerStats>().rockGlowCount + " and max glow at :  " + gm.GetComponent<PlayerStats>().maxGlowRocks);
                        return;
                    }
                }
            }
        }
    }
    
    void Update()
    {
        if (maxedGlow && (Time.time - timeSinceLastContaminationAttempt > 2.0f) && CanCharge)
        {
            timeSinceLastContaminationAttempt = Time.time;
            int chance = Random.Range(0, 50);
            //There is a 1 in 20 chance that every second another wock will be contaminated
            if(chance == 44)
            {
                ContaminateRock();
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
                    gm.GetComponent<PlayerStats>().rockGlowCount -= 1;
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
        //rend.material.Lerp(material1, material2, Chargelevel / 100);
        if((this.transform.position - player.transform.position).magnitude > ((gm.GetComponent<PlayerStats>().finalHealth * 7) / 110))
        {
            rend.material.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            rend.material.Lerp(material1, material2, Chargelevel / 100);
        }
    }
}
