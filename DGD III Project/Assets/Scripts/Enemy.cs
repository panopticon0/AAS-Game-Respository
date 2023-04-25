using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health = 10.0f;
    private float speed = 5f;
    private Transform player;
    public float range = 1.5f;
    private float deathTime = 1.0f;
    public bool kill = false;
    public Animator enemyAnim;
    private Vector3 startPos;
    public Transform head;
    private CharacterController enemyAI;

    public float gravityValue = 3f;
    public float gravityMultiplier = 1.1f;
    public float enemyYVelocity = 3f;
    [SerializeField] private AudioSource ZombieAttack;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemyAnim = GetComponent<Animator>();
        startPos = transform.position;
        enemyAI = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        Vector3 velocity = direction * speed;

        float gravity = gravityValue * gravityMultiplier * Time.deltaTime;
        float distance = Vector3.Distance(player.position, transform.position);
        
        if (distance <= range)
        {
            if (enemyAI.isGrounded)
            {
                Debug.Log("True " + enemyYVelocity);
                enemyYVelocity = 0f;
            } else
            {
                enemyYVelocity -= gravity;
            }
            velocity.y = enemyYVelocity;
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            velocity.Normalize();
            enemyAnim.SetBool("playerDetected", true);
            enemyAI.Move(speed * velocity * Time.deltaTime);
        } else
        {
            enemyAnim.SetBool("playerDetected", false);
            if (Vector3.Distance(startPos, transform.position) >= 2)
            {
                direction = startPos - transform.position;
                velocity = direction * speed;
                enemyAnim.SetBool("playerDetected", true);
                if (enemyAI.isGrounded)
                {
                    enemyYVelocity = 0f;
                }
                else
                {
                    enemyYVelocity -= gravity;
                }
                velocity.y = enemyYVelocity;
                transform.LookAt(new Vector3(startPos.x, transform.position.y, startPos.z));
                velocity.Normalize();
                enemyAI.Move(speed * velocity * Time.deltaTime);
            }
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
            health -= other.gameObject.GetComponent<Bullet>().damage;
            Destroy(other.gameObject);
            Debug.Log(health);

        } else if (other.gameObject.tag == "Melee")
        {
            health -= other.gameObject.GetComponent<Swing>().damage;
            Destroy(other.gameObject);
            Debug.Log(health);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Attacking!");
            enemyAnim.SetBool("attacking", true);
            ZombieAttack.Play();

        }
    }

    void OnCollisionExit(Collision collision2)
    {
        if (collision2.gameObject.tag == "Player")
        {
            enemyAnim.SetBool("attacking", false);
        }
    }
}
