using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations.Rigging;


public class Head : MonoBehaviour
{



    public float camSpeed = 3.0f;
    public float upperX = 90.0f;
    public float lowerX = 345.0f;
    public TextMeshProUGUI bulletDisplay;
    public TextMeshProUGUI weaponText;
    
    [SerializeField] private AudioSource shotsSoundEffect;
    [SerializeField] private AudioSource reloadSoundEffect;

    //how long it takes to fire another shot
    private List<float> recoil = new List<float>();
    //maximum stock of bullets
    private List<float> max = new List<float>();
    //stock of bullets
    private List<float> stock = new List<float>();
    //how long it takes to reload bullets
    private List<float> reload = new List<float>();
    //time before recovering recoil
    private List<float> recover = new List<float>();
    //time before reloading all bullets
    private List<float> recoverB = new List<float>();
    //shotgun accuracy range
    private float rangeRotate = 3.0f;
    //how many bullets to fire in a single shot
    private float bulletLoadout = 3.0f;
    //enemy detection range
    private float range = 10.0f;
    private List<GameObject> enemies = new List<GameObject>();
    private int target = 0;

    public Transform bulletPosition;
    public RigBuilder weaponRig;
    public int[] weaponLayers = { 0, 1, 2, 3, 4 };
    public List<GameObject> weapons = new List<GameObject>();
    public string[] weaponNames = { "Pistol", "Flintlock", "Assault Rifle", "Shotgun", "Katana" };
    public GameObject particleFlash;

        public GameObject player;
        public GameObject camera;
        //camera object
        public Transform pos1;
        //default position of the camera object
        public Transform pos2;
        //dynamic aiming position of the camera object
        public List<GameObject> bullet = new List<GameObject>();
        //what items the player has
        public List<bool> have = new List<bool>();
        //selected weapon id
        public float select = 0f;
        // Start is called before the first frame update
        void Start()
        {

            recoil.Add(0.15f);
            recoil.Add(1.0f);
            recoil.Add(0.04f);
            recoil.Add(0.7f);
            recoil.Add(0.7f);

            reload.Add(1.0f);
            reload.Add(2.0f);
            reload.Add(1.5f);
            reload.Add(1.5f);
            reload.Add(0.0f);

            max.Add(6.0f);
            max.Add(2.0f);
            max.Add(20.0f);
            max.Add(4.0f);
            max.Add(1.0f);

            for (int i = 0; i < bullet.Count; i++)
            {
                stock.Add(max[i]);
                recover.Add(recoil[i]);
                recoverB.Add(reload[i]);
                have.Add(false);
            }

            //set initial gun to true, have at least one weapon at all times
            have[0] = true;
            
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            bulletDisplay.text = "AMMO: " + stock[(int)select].ToString();
            if (stock[(int)select] == 0)
            {
                bulletDisplay.text = "AMMO: Reloading";
                reloadSoundEffect.Play();
            }
            //head rotation
            camSpeed = 3.0f;
            transform.position = player.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
            float verticalVInput = Input.GetAxis("Mouse Y");
            float horizontalVInput = Input.GetAxis("Mouse X");
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + (camSpeed * verticalVInput * -1), transform.eulerAngles.y, transform.eulerAngles.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (camSpeed * horizontalVInput), transform.eulerAngles.z);
            if (transform.eulerAngles.x <= lowerX && transform.eulerAngles.x > upperX)
            {
                transform.eulerAngles = new Vector3(lowerX + 1, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else if (transform.eulerAngles.x >= upperX && lowerX > transform.eulerAngles.x)
            {
                transform.eulerAngles = new Vector3(upperX - 1, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
            //lock onto target
            checkRange();
            if (target >= enemies.Count)
            {
                target = 0;
            }
            if (target < 0)
            {
                target = enemies.Count - 1;
            }
            if (Input.GetKey(KeyCode.LeftShift) && enemies.Count > 0 && enemies[target].GetComponent<Enemy>().kill == false)
            {
                if (enemies[target].GetComponent<Enemy>().kill == false)
                {
                    transform.LookAt(enemies[target].GetComponent<Enemy>().head.position);
                }
                else
                {
                    target++;
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    target++;
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    target--;
                }
                if (Vector3.Distance(pos2.position, camera.transform.position) >= .001f)
                {
                    camera.transform.position = Vector3.MoveTowards(camera.transform.position, pos2.position, 8.0f * Time.deltaTime);
                    //if holding left shift and camera is not at position 2, move camera to position 2
                }
            }
            else if (Vector3.Distance(pos1.position, camera.transform.position) >= .001f)
            {
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, pos1.position, 8.0f * Time.deltaTime);
                //if not holding left shift and camera is not at position1, move camera to position 1
            }
            if (select < 0)
            {
                select = bullet.Count - 1;
            }
            if (select >= bullet.Count)
            {
                select = 0;
            }
            changeWeapon();
            weaponSwitch();
        //shooting bullets
        if (Input.GetMouseButtonDown(0) && stock[(int)select] > 0 && recover[(int)select] >= recoil[(int)select])
            {
                shotsSoundEffect.Play();
                if (select == 4)
                {
                    GameObject punch = Instantiate(bullet[(int)select], player.transform, false);
                    punch.transform.localPosition += new Vector3(-0.5f, 1.75f, 0.55f);
                }
                else if (select == 3)
                {
                    for (int i = 0; i < bulletLoadout; i++)
                    {
                        Instantiate(bullet[(int)select], bulletPosition.position, bulletPosition.rotation * Quaternion.Euler(Random.Range(-rangeRotate, rangeRotate), Random.Range(-rangeRotate, rangeRotate), 0));
                        Instantiate(particleFlash, bulletPosition.position, bulletPosition.rotation * Quaternion.Euler(Random.Range(-rangeRotate, rangeRotate), Random.Range(-rangeRotate, rangeRotate), 0));
                    }
                }
                else
                {
                    Instantiate(bullet[(int)select], bulletPosition.position, bulletPosition.rotation);
                    Instantiate(particleFlash, bulletPosition.position, bulletPosition.rotation * Quaternion.Euler(Random.Range(-rangeRotate, rangeRotate), Random.Range(-rangeRotate, rangeRotate), 0));
                }
                recover[(int)select] = 0.0f;
                stock[(int)select] -= 1;
                recoverB[(int)select] = reload[(int)select];
            }
            if (recover[(int)select] < recoil[(int)select])
            {
                recover[(int)select] += Time.deltaTime;
            }
            if (stock[(int)select] <= 0)
            {
                recoverB[(int)select] -= Time.deltaTime;
                if (recoverB[(int)select] <= 0)
                {
                    stock[(int)select] = max[(int)select];
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //check for enemy in range
            if (other.tag == "Enemy")
            {
                bool exist = false;
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (other.gameObject.GetInstanceID() == enemies[i].GetInstanceID() || other.gameObject.GetComponent<Enemy>().kill == true)
                    {
                        exist = true;
                    }
                }
                if (exist == false)
                {
                    enemies.Add(other.gameObject);
                    Debug.Log("Found target");
                }
            }
        }
        
        private void changeWeapon()
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                if (select + 1f >= bullet.Count)
                {
                    select = 0;
                }
                else
                {
                    select++;
                }
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                if (select - 1f < 0)
                {
                    select = bullet.Count - 1;
                }
                else
                {
                    select--;
                }
            }
            //if weapon is marked as false, automatically scroll through weapons until you pick one that is true.
            while (have[(int)select] == false)
            {
                changeWeapon();
            }
        }
        
        private void checkRange()
        {
            //check for enemy out of range or deleted
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    Debug.Log("removed target");
                    i--;
                }
                if (enemies.Count > 0)
                {
                    float distance = Vector3.Distance(enemies[i].transform.position, transform.position);
                    if (distance > range)
                    {
                        enemies.RemoveAt(i);
                        Debug.Log("Removed Target");
                        i--;
                    }
                }
            }
        }
   
        private void weaponSwitch()
        {
            //enable rig layer and model of equipped weapon
            weaponRig.layers[(int)select].active = true;
            weapons[(int)select].SetActive(true);
            //turn off other layers and models if they do not correspond to the weapon id
            foreach (int id in weaponLayers)
            {
                if (id != (int)select)
                {
                    weaponRig.layers[id].active = false;
                }
            }

        foreach (GameObject weapon in weapons)
        {
            int number = weapons.IndexOf(weapon);
            if ( number != (int)select)
            {
               weapons[number].SetActive(false);
            }
        }

        weaponText.text = "Weapon: " + weaponNames[(int)select].ToString();
    }
}
