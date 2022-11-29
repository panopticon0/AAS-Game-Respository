using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool onGround = true;
    public float speed = 5.0f;
    private float thrust = 4.0f;
    private Rigidbody playerRb;
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, camera.transform.eulerAngles.y, transform.eulerAngles.z);
        transform.Translate(Vector3.forward * speed * Time.deltaTime * verticalInput);
        transform.Translate(Vector3.right * speed * Time.deltaTime * horizontalInput);

        if (Input.GetKeyDown("space") && onGround == true)
        {
            playerRb.AddForce(0, thrust, 0, ForceMode.Impulse);
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
