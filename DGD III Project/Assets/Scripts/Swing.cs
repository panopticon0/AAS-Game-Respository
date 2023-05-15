using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    private float speed = 5.0f;
    private float timer = 0.25f;
    public float damage = 2f;
    private Transform player;
    [SerializeField] private AudioSource swingSound;

    // Start is called before the first frame update
    void Start()
    {
        swingSound.Play();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, player);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
