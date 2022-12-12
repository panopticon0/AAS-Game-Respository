using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health = 5.0f;
    private float speed = 2.5f;
    private Transform player;
    public float range = 1.5f;
    private float deathTime = 1.0f;
    public bool kill = false;
    public Animator enemyAnim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= range)
        {
            transform.LookAt(player);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            enemyAnim.SetBool("playerDetected", true);
        } else
        {
            enemyAnim.SetBool("playerDetected", false);
        }
        if (health <= 0)
        {
            kill = true;
            speed = 0.0f;
            deathTime -= Time.deltaTime;
        } if (deathTime < 0)
        {
            Destroy(gameObject);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
            health--;

        } else if (other.gameObject.tag == "Player")
        {
            Debug.Log("Attacking!");
            enemyAnim.SetBool("attacking", true);
        }

    }


}
