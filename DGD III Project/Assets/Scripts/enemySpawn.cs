using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    //radius that enemies can spawn from spawner object position
    private float radius = 15f;
    //number of enemies that the object should spawn
    public int numEne = 5;
    //types of enemies the spawner should spawn
    public List<GameObject> enemyType;
    // Start is called before the first frame update
    public bool spawn = false;

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (spawn == true)
        {
            for (int i = 0; i < numEne; i++)
            {
                //repeatedly spawn enemies in random positions within the range until i is equal to number of enemies that the object should spawn
                Instantiate(enemyType[(int)Random.Range(0, 2)], transform.position + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius)), transform.rotation);
                Debug.Log(i);
            } spawn = false;
        }
    }
}
