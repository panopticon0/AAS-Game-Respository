using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public bool onGround = true;
    public float speed = 5.0f;
    public float thrust = 4.0f;
    public float rotationSpeed;
    public Rigidbody playerRb;
    public Animator playerAnim;
    public GameObject head;

    
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
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, head.transform.eulerAngles.y, transform.eulerAngles.z);




        if (Input.GetKeyDown("space") && onGround == true)
        {
            playerRb.AddForce(0, thrust, 0, ForceMode.Impulse);
        }
        
      

        //Animation code, cleaning up after exam/presentation
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) )
        {

            playerAnim.SetFloat("vertical", 1.0f);
        } else
        {
            playerAnim.SetFloat("vertical", 0.0f);
        }
        //Left Shift to activate run animation and increase speed
        if (Input.GetKey(KeyCode.RightShift))
        {
            playerAnim.SetBool("running", true);
            speed = 7.5f;

        }else if (Input.GetKeyUp(KeyCode.RightShift))
        {
            playerAnim.SetBool("running", false);
            speed = 5.0f;
        }
        //End Animation code
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
