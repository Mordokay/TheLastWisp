using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    //min healt is 0 - spotAngle is 10
    //max helath is 130 - spotAngle is 140
    private Light spotLight;
    public float health;

    private void Awake()
    {
        spotLight = GetComponentInChildren<Light>();
    }

    private void Start()
    {
        health = spotLight.spotAngle - 10;
    }

    public bool isLimitedByHealth()
    {
        return health <= 10;
    }

    public void UpdateHealth()
    {

    }
}
