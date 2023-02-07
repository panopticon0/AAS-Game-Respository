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
    public float health = 5.0f;
    private bool invi = false;
    public float timer = 5f;
    public float gravMultiplier = 2f;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(gravMultiplier * Physics.gravity, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        transform.Translate(new Vector3(horizontalInput * speed, 0, verticalInput * speed) * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, head.transform.eulerAngles.y, transform.eulerAngles.z);




        if (Input.GetKeyDown("space") && onGround == true)
        {
            playerRb.AddForce(0, thrust, 0, ForceMode.Impulse);
        }



        //Animation code, cleaning up after exam/presentation
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {

            playerAnim.SetFloat("vertical", 1.0f);
        }
        else
        {
            playerAnim.SetFloat("vertical", 0.0f);
        }
        //Left Shift to activate run animation and increase speed
        if (Input.GetKey(KeyCode.RightShift))
        {
            playerAnim.SetBool("running", true);
            speed = 7.5f;

        }
        else if (Input.GetKeyUp(KeyCode.RightShift))
        {
            playerAnim.SetBool("running", false);
            speed = 5.0f;
        }
        //End Animation code
        if (invi == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                invi = false;
                timer = 5.0f;
                Debug.Log("invi set to" + invi);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (invi == false)
            {
                health--;
                invi = true;
                Debug.Log(health + " invi set to true");
            }
        } if (collision.gameObject.tag == "Pickup")
        {
            //if the collision is registered as a pickup item, get the name of the collision, check what integer it is named as and mark it in the array as true
            //ex. if collide with a pickup item named "1", have[1] in head script will be marked true
            head.GetComponent<Head>().have[int.Parse(collision.gameObject.name, System.Globalization.NumberStyles.Integer)] = true;
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionExit(Collision collision2)
    {
        if (collision2.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }
}
