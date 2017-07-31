using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabParticleSystem : MonoBehaviour {

    public GameObject prefabObject;
    public int maxNumberParticles;
    public float scatterSpeed;

    private void Start()
    {
        int particleCount = Random.Range(0, maxNumberParticles);
        for(int i = 0; i < particleCount; i++)
        {
            GameObject myPrefab = Instantiate(prefabObject);
            myPrefab.transform.parent = this.transform;
            myPrefab.transform.position = this.transform.position;
            myPrefab.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(0.0f, 1.0f), 0.0f,
                Random.Range(0.0f, 1.0f)) * scatterSpeed;
            Destroy(myPrefab, Random.Range(3.0f, 7.0f));
        }
        Destroy(this.gameObject, 7.0f);
    }
    void Update () {
        		
	}
}
