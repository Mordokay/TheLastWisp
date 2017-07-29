using UnityEngine;
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
                    }
                }
            }
        }
    }
}
