using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
    public GameObject boundary;
    public GameObject ground;
    public GameObject redZone;
    public bool hideObjects;

    public Vector2 mapCenter = Vector2.zero;

    float[,] noiseMap;

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
        seed = PlayerPrefs.GetInt("seed");
        ground.transform.localScale = new Vector3(sizeX, ground.transform.localScale.y, sizeY);
        redZone.transform.localScale = new Vector3(sizeX+20, redZone.transform.localScale.y, sizeY+20);

        if(sizeX % 2 == 0)
        {
            sizeX += 1;
        }

        if (sizeY % 2 == 0)
        {
            sizeY += 1;
        }

        CreateHeight();
        CreateColliders();
    }

    private void CreateColliders()
    {
        BoxCollider collider;
        float offset = 0.5f;

        GameObject leftbound = Instantiate(boundary) as GameObject;
        leftbound.transform.position = new Vector3((sizeX/2) + offset, 0, 0);
        leftbound.transform.parent = this.transform;
        collider = leftbound.GetComponent<BoxCollider>();
        collider.size = new Vector3(1, collider.size.y, sizeY+10);

        GameObject upperbound = Instantiate(boundary) as GameObject;
        upperbound.transform.position = new Vector3(0, 0, (-sizeY / 2)- offset);
        upperbound.transform.parent = this.transform;
        collider = upperbound.GetComponent<BoxCollider>();
        collider.size = new Vector3(sizeX+10, collider.size.y, 1);

        GameObject rightbound = Instantiate(boundary) as GameObject;
        rightbound.transform.position = new Vector3((-sizeX / 2) - offset, 0, 0);
        rightbound.transform.parent = this.transform;
        collider = rightbound.GetComponent<BoxCollider>();
        collider.size = new Vector3(1, collider.size.y, sizeY+10);

        GameObject lowerbound = Instantiate(boundary) as GameObject;
        lowerbound.transform.position = new Vector3(0, 0, (sizeY / 2) + offset);
        lowerbound.transform.parent = this.transform;
        collider = lowerbound.GetComponent<BoxCollider>();
        collider.size = new Vector3(sizeX+10, collider.size.y, 1);
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

                                myObj.GetComponent<RockBlend>().CanCharge = true;
                                myObj.GetComponent<RockBlend>().Chargelevel = 100;
                                myObj.GetComponent<RockBlend>().maxedGlow = true;
                            }
                            else
                            {
                                myObj.GetComponent<RockBlend>().CanCharge = false;
                                myObj.GetComponent<RockBlend>().Chargelevel = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}
