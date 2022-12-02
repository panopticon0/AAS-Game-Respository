using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool onGround = true;
    public float speed = 5.0f;
    private float thrust = 4.0f;
    public Rigidbody playerRb;
    public Animator playerAnim;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontalInput * speed * Time.deltaTime, 0, verticalInput * speed * Time.deltaTime);
      
        if (Input.GetKeyDown("space") && onGround == true)
        {
            playerRb.AddForce(0, thrust, 0, ForceMode.Impulse);
        }

        if (verticalInput >= 1)
        {
            playerAnim.SetFloat("vertical", 0.5f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
    }

    void OnCollisionExit (Collision collision2)
    {
        if (collision2.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }
}
