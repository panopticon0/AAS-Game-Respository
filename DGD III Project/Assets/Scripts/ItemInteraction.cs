using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ItemInteraction : MonoBehaviour
{
    
   public TextMeshPro itemText;
    public GameObject textObj;
    // Start is called before the first frame update
    void Start()
    {
        itemText = textObj.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Food")
        {
          itemText.meshRenderer.enabled = true;
        }
    }
}

