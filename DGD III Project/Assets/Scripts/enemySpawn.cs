using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    private float radius = 15f;
    public int numEne = 5;
    public List<GameObject> enemyType;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numEne; i++)
        {
            Instantiate(enemyType[(int)Random.Range(0, 2)], transform.position + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius)), transform.rotation);
            Debug.Log(i);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
