using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{ 
    public Transform player;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        transform.LookAt(player);
    }
}
