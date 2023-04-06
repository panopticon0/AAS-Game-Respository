using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
    public int planks = 0;
    public bool planksComplete = false;
    public TextMeshProUGUI itemText;
    public GameObject textObj;
    public bool itemConsume = false;
    public bool consumed = false;
    public bool plankCollect = false;

    public TextMeshProUGUI endTriggerText;
    public GameObject endButton;
    public HealthBar healthBar;
    


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        itemText = textObj.GetComponent<TextMeshProUGUI>();
        healthBar.SetMaxHealth(health);

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



        //Animation code
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {

            playerAnim.SetFloat("vertical", 1.0f);
            if (Input.GetKey(KeyCode.RightShift))
            {
                //Right Shift to activate run animation and increase speed
                playerAnim.SetBool("running", true);
                speed = 7.5f;

            } else
            {
                playerAnim.SetBool("running", false);
                speed = 5.0f;
            }
        }
        else
        {
            playerAnim.SetFloat("vertical", 0.0f);
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

        //Player presses E to consume item
        if (itemConsume == true && Input.GetKeyDown(KeyCode.E))
        {
                health = health + 3.0f;
                consumed = true;
        } 
        
        if (plankCollect == true && Input.GetKeyDown(KeyCode.E))
        {
            planks++;
            consumed = true;
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
                healthBar.SetHealth(health);
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food" || other.gameObject.tag == "Health")
        {
            itemConsume = true;
            consumed = false;
            plankCollect = false;
        }

        if (other.gameObject.tag == "Plank")
        {
            plankCollect = true;
            consumed = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Food" || other.gameObject.tag == "Health")
        {
            itemText.enabled = false;
            itemConsume = false;
            consumed = false;
        }
        
        if (other.gameObject.tag == "Plank")
        {
            itemText.enabled = false;
            itemConsume = false;
            consumed = false;
            plankCollect = false;
        }

        if (other.gameObject.tag == "EndingTrigger")
        {
            endTriggerText.enabled = false;
            endButton.SetActive(false);
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {

            itemText.text = "Press E to Eat";
            itemText.enabled = true;


        }
        else if (other.gameObject.tag == "Health")
        {

            itemText.text = "Press E to Use";
            itemText.enabled = true;


        }
        else if (other.gameObject.tag == "Plank")
        {

            itemText.text = "Press E to Collect";
            itemText.enabled = true;

        }

        if (consumed == true && (other.gameObject.tag == "Food" || other.gameObject.tag == "Health" || other.gameObject.tag == "Plank"))
        {
            itemText.enabled = false;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "EndingTrigger")
        {
            endButton.SetActive(true);
            endTriggerText.enabled = true;

            if (planks >= 12)
            {
                planksComplete = true;
                endButton.SetActive(true);
                endTriggerText.enabled = false;

            } else
            {
                endButton.SetActive(false);
                endTriggerText.enabled = true;
                endTriggerText.text = "You need " + (12 - planks) + " more planks to escape!";
            }
        }

    }


}
