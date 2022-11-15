using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{ 
    public Transform player;
    private float camSpeed;
    public float upperX = 90.0f;
    public float lowerX = 345.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        camSpeed = 20.0f;
    }

    void LateUpdate()
    {
        float horizontalVInput = Input.GetAxis("Mouse X");
        float verticalVInput = Input.GetAxis("Mouse Y");
        transform.LookAt(player);
    }
}
