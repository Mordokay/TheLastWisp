﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapTerrainGenerator : MonoBehaviour {

    public int sizeX;
    public int sizeY;

    public Noise.NormalizeMode normalizedMode;
    [Tooltip("Generates a map based on a number")]
    public int seed = 1234;
    public Vector2 offset;
    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunarity;

    public bool hideObjects;

    public Vector2 mapCenter = Vector2.zero;

    float[,] noiseMap;

    GameObject gm;

    [System.Serializable]
    public class NoiseElement{

        [SerializeField]
        public GameObject myObject;
        [SerializeField]
        public float minTheshhold;
        [SerializeField]
        public float maxTheshold;
        [SerializeField]
        public float upShift;
        [SerializeField]
        public bool isRock;
        [SerializeField]
        public bool canCharge;
    }

    [SerializeField]
    public List<NoiseElement> myItems;

    void Reset()
    {
        sizeX = 40;
        sizeY = 40;
        seed = 12345;
        noiseScale = 5;
        octaves = 2;
        persistence = 0.56f;
        lacunarity = 0;
        mapCenter = Vector2.zero;
        offset = Vector2.zero;
        normalizedMode = Noise.NormalizeMode.Global;
    }

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        seed = PlayerPrefs.GetInt("seed");
        gm.GetComponent<PlayerStats>().rocks = new int[sizeX, sizeY];
        gm.GetComponent<PlayerStats>().rockObjects = new GameObject[sizeX, sizeY];
        CreateHeight();
    }

    void OnInspectorGUI()
    {
        Debug.Log("Changed something!!!");
        //CreateHeight();
    }

    void CreateHeight()
    {
        noiseMap = Noise.GeneratedNoiseMap(sizeX, sizeY, seed, noiseScale, octaves, persistence, lacunarity, mapCenter + offset, normalizedMode);

        for(int i = 0; i < sizeX ; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                float greyColor = noiseMap[i, j];
                //Instanciate  object
                foreach (NoiseElement el in myItems)
                {
                    if (greyColor > el.minTheshhold && greyColor < el.maxTheshold)
                    {
                        GameObject myObj = Instantiate(el.myObject) as GameObject;
                        myObj.transform.position = new Vector3(i - sizeX / 2, 0.0f, j - sizeY / 2);
                        myObj.transform.Translate(Vector3.up * el.upShift);
                        myObj.transform.parent = this.transform;
                        if (el.isRock) {
                            if (el.canCharge)
                            {
                                gm.GetComponent<PlayerStats>().rocks[i, j] = 1;
                                gm.GetComponent<PlayerStats>().rockObjects[i, j] = myObj;
                                gm.GetComponent<PlayerStats>().rockGlowCount += 1;
                                myObj.GetComponent<RockBlend>().CanCharge = true;
                                myObj.GetComponent<RockBlend>().Chargelevel = 100;
                                myObj.GetComponent<RockBlend>().maxedGlow = true;
                            }
                            else
                            {
                                gm.GetComponent<PlayerStats>().rocks[i, j] = 2;
                                gm.GetComponent<PlayerStats>().rockObjects[i, j] = myObj;
                                gm.GetComponent<PlayerStats>().rockNormalCount += 1;
                                myObj.GetComponent<RockBlend>().CanCharge = false;
                                myObj.GetComponent<RockBlend>().Chargelevel = 0;
                            }
                        }
                    }
                }
            }
        }

        gm.GetComponent<PlayerStats>().maxGlowRocks = (int)(gm.GetComponent<PlayerStats>().rockGlowCount * 1.5f);
    }
}
