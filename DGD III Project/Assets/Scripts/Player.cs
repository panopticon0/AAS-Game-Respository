using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Player : MonoBehaviour
{

    public bool onGround = true;
    public float speed = 5.0f;
    public float thrust = 4.0f;
    public float rotationSpeed;
    //public Rigidbody playerRb;
    public CharacterController playerAI;
    public Animator playerAnim;
    public GameObject head;
    public float playerHealth = 20.0f;
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
    public TextMeshProUGUI plankText;
    private float playerVelocity = 0f;

    public TextMeshProUGUI endTriggerText;
    public GameObject deathPanel;
    public GameObject endButton;
    public HealthBar healthBar;

    private RigBuilder rigBuilder;
    [SerializeField] private AudioSource collectionSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;


    


    // Start is called before the first frame update
    void Start()
    {
        //playerRb = GetComponent<Rigidbody>();
        playerAI = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();
        itemText = textObj.GetComponent<TextMeshProUGUI>();
        healthBar.SetMaxHealth(playerHealth);
        rigBuilder = GetComponent<RigBuilder>();

    }

    public void FixedUpdate()
    {
        //GetComponent<Rigidbody>().AddForce(gravMultiplier * Physics.gravity, ForceMode.Acceleration);
        if (!playerAI.isGrounded)
        {
            playerVelocity -= 7.81f * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (playerHealth > 0)
        {
            if (playerAI.isGrounded)
            {
                if (Input.GetKeyDown("space"))
                {
                    playerVelocity += thrust;
                    playerAnim.SetBool("jumping", true);
                } else
                {
                    playerVelocity = 0f;
                    playerAnim.SetBool("jumping", false);
                }
            }

            //transform.Translate(new Vector3(horizontalInput * speed, prevVelocity, verticalInput * speed) * Time.deltaTime); ;
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
            movement = transform.TransformDirection(movement);
            movement.Normalize();
            movement.y = playerVelocity;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, head.transform.eulerAngles.y, transform.eulerAngles.z);
            playerAI.Move(movement * Time.deltaTime * speed);
        }

        healthBar.SetHealth(playerHealth);

        //death sequence
        if (playerHealth <= 0)
        {
            playerAnim.SetBool("dead", true);
            GameObject.Find("HeadCube (1)").GetComponent<Head>().enabled = false;
            rigBuilder.enabled = false;
            deathPanel.SetActive(true);
        }


        //Animation code
        if ( Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) )
        {

            playerAnim.SetFloat("vertical", 1.0f);
            if (Input.GetKey(KeyCode.LeftControl))
            {
                //Right Shift to activate run animation and increase speed
                playerAnim.SetBool("running", true);
                speed = 8.0f;

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
            playerHealth = playerHealth + 3.0f;
                consumed = true;
        } 
        
        if (plankCollect == true && Input.GetKeyDown(KeyCode.E))
        {
            planks++;
            consumed = true;
            plankCollect = false;
        }

        plankText.text = "PLANKS: " + (planks);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            deathSoundEffect.Play();
            if (invi == false)
            {
                playerHealth--;
                healthBar.SetHealth(playerHealth);
                invi = true;
                Debug.Log(playerHealth + " invi set to true");
            }
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

        if (other.gameObject.tag == "Pickup")
        {
            Debug.Log("Weapon");
            //if the collision is registered as a pickup item, get the name of the collision, check what integer it is named as and mark it in the array as true
            //ex. if collide with a pickup item named "1", have[1] in head script will be marked true
            head.GetComponent<Head>().have[int.Parse(other.gameObject.name, System.Globalization.NumberStyles.Integer)] = true;
            collectionSoundEffect.Play();
            Destroy(other.gameObject);
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
                Cursor.visible = true;

            } else
            {
                endButton.SetActive(false);
                endTriggerText.enabled = true;
                endTriggerText.text = "You need " + (12 - planks) + " more planks to escape!";
            }
        }

       

    }


    public void restartScene()
    {
        SceneManager.LoadScene("Main city-forest");
    }

    public void quit()
    {
        Application.Quit();

    }

}
