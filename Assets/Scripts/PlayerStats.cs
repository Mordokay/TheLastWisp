using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public bool droppingBeacon = true;
    public float lightChangeSpeed;
    GameObject player;
    bool isLosingHealth;
    float finalHealth;
    bool isGainingHealth;
    public Light playerLight;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        finalHealth = playerLight.spotAngle;
    }

    private void Update()
    {
        if (finalHealth != playerLight.spotAngle)
        {
            if (finalHealth < playerLight.spotAngle)
            {
                isLosingHealth = true;
                isGainingHealth = false;
            }
            else {
                isGainingHealth = true;
                isLosingHealth = false;
            }
        }

        if (isLosingHealth)
        {
            if (playerLight.spotAngle <= finalHealth)
            {
                playerLight.spotAngle = finalHealth;
                isLosingHealth = false;
            }
            else
            {
                playerLight.spotAngle -= Time.deltaTime * lightChangeSpeed;
            }
        }

        else if (isGainingHealth)
        {
            if (playerLight.spotAngle >= finalHealth)
            {
                playerLight.spotAngle = finalHealth;
                isGainingHealth = false;
            }
            else
            {
                playerLight.spotAngle += Time.deltaTime * lightChangeSpeed;
            }
        }

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
    
    public bool isLowHealth()
    {
        return finalHealth <= 20;
    }

    public bool isHighHealth()
    {
        return finalHealth >= 110;
    }

    public void LoseHealth(float amount)
    {
        finalHealth  -= amount;
        if (finalHealth < 20)
        {
            finalHealth = 20;
        }
    }

    public void GainHealth(float amount)
    {
        finalHealth += amount;
        if(finalHealth > 110)
        {
            finalHealth = 110;
        }
    }
}
