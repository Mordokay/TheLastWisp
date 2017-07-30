using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public bool droppingBeacon = true;
    public float lightDecreaseSpeed;
    Light playerHealth;
    GameObject player;
    bool isLosingHealth;
    float finalHealth;
    bool isGainingHealth;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.gameObject.GetComponentInChildren<Light>();
        finalHealth = playerHealth.spotAngle;
    }

    private void Update()
    {
        if(finalHealth!= playerHealth.spotAngle)
        {
            if(finalHealth < playerHealth.spotAngle)
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
            if(playerHealth.spotAngle <= finalHealth)
            {
                playerHealth.spotAngle = finalHealth;
                isLosingHealth = false;
            }
            else
            {
                playerHealth.spotAngle -= Time.deltaTime * lightDecreaseSpeed;
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
        return finalHealth <= 30;
    }

    public void LoseHealth(float amount)
    {
        finalHealth  -= amount;
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
