using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour   
{
    public float camSpeed = 3.0f;
    public float upperX = 90.0f;
    public float lowerX = 345.0f;

    public float recoil = 0.15f;
    public float max = 6.0f;
    private float stock = 0.0f;
    public float reload = 1.0f;
    private float recover = 0.0f;
    private float recoverB = 0.0f;

    public GameObject player;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        stock = max;
        recover = recoil;
        recoverB = reload;
    }

    // Update is called once per frame
    void Update()
    {
        camSpeed = 3.0f;
        transform.position = player.transform.position + new Vector3(0.0f, 0.6f, 0.0f);
        float verticalVInput = Input.GetAxis("Mouse Y");
        float horizontalVInput = Input.GetAxis("Mouse X");
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + (camSpeed * verticalVInput), transform.eulerAngles.y, transform.eulerAngles.z);
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
        if (Input.GetMouseButtonDown(0) && stock > 0 && recover >= recoil)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            recover = 0.0f;
            stock -= 1;
            recoverB = reload;
        }
        if (recover < recoil)
        {
            recover += Time.deltaTime;
        }
        if (stock <= 0)
        {
            recoverB -= Time.deltaTime;
            if (recoverB <= 0)
            {
                stock = max;
            }
        }
    }
}
