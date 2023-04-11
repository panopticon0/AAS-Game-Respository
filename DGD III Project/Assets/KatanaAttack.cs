using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaAttack : MonoBehaviour
{
    Animator katanaAnim;
    // Start is called before the first frame update
    void Start()
    {
        katanaAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            katanaAnim.SetTrigger("Attack");
        }
    }
}
