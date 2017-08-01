using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public bool droppingBeacon = true;
    public bool droppingBarrier = true;
    public float lightChangeSpeed;
    GameObject player;
    bool isLosingHealth;
    public float finalHealth;
    bool isGainingHealth;
    public Light playerLight;
    public Image deposit;
    public Image beaconButton;
    public Image barrierButton;
    public List<ButtonLayouts> beaconButtonLayouts;
    public List<ButtonLayouts> barrierButtonLayouts;
    public int[,] rocks;
    public GameObject[,] rockObjects;
    public int rockNormalCount = 0;
    public int rockGlowCount = 0;
    public int maxGlowRocks = 0;

    public List<GameObject> beacons;

    public float playerXP;
    public float XPIncreaseEachLevel;
    public int playerLevel;
    public float XPToNextLevel;

    public int upgradePoints;
    public int barrierLevel;
    public int beaconLevel;
    public int grenadeLevel;

    public int enemyCount;
    public int enemiesKilled;

    public float life;

    private void Start()
    {
        life = 100;
        enemiesKilled = 0;
        playerXP = 0.0f;
        playerLevel = 1;
        barrierLevel = 1;
        beaconLevel = 1;
        grenadeLevel = 1;
        XPToNextLevel = XPIncreaseEachLevel;
        upgradePoints = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        finalHealth = playerLight.spotAngle;
        beacons = new List<GameObject>();
    }

    private void Update()
    {
        life = Mathf.Clamp(life, 0.0f, 100.0f);
        if(life == 0.0f)
        {
            Debug.Log("Player Lose Game");
        }
        if (XPToNextLevel < playerXP)
        {
            playerXP -= XPToNextLevel;
            playerLevel += 1;
            XPToNextLevel += XPIncreaseEachLevel;
            upgradePoints += 1;
        }

        if (upgradePoints > 0)
        {
            //Removes upgrade point after selecting what upgrade the player wants
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                beaconLevel += 1;
                upgradePoints -= 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)){
                upgradePoints -= 1;
                barrierLevel += 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)){
                upgradePoints -= 1;
                grenadeLevel += 1;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                beaconButton.sprite = beaconButtonLayouts[beaconLevel].layouts[2];
                player.GetComponent<ItemDropper>().CleanBarriers();
                droppingBarrier = false;
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

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                beaconButton.sprite = beaconButtonLayouts[beaconLevel].layouts[1];
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && upgradePoints == 0)
            {
                barrierButton.sprite = barrierButtonLayouts[barrierLevel].layouts[2];
                droppingBeacon = false;
                player.GetComponent<ItemDropper>().CleanBeacons();
                if (droppingBarrier)
                {
                    droppingBarrier = false;
                    player.GetComponent<ItemDropper>().CleanBarriers();
                }
                else
                {
                    droppingBarrier = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                barrierButton.sprite = barrierButtonLayouts[barrierLevel].layouts[1];
            }
        }
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
        life -= amount;
        if (finalHealth < 20)
        {
            finalHealth = 20;
        }
    }

    public void GainHealth(float amount)
    {
        finalHealth += amount;
        life += amount;
        if (finalHealth > 110)
        {
            finalHealth = 110;
            
            if (deposit.fillAmount < 1 && (deposit.fillAmount + amount/100) <= 1)
            {
                
                deposit.fillAmount += amount/100;
            }
            else { deposit.fillAmount = 1; }
        }
    }
}
