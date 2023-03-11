using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInteraction : MonoBehaviour
{

    public TextMeshProUGUI itemText;
    public GameObject textObj;
    // Start is called before the first frame update
    void Start()
    {
        itemText = textObj.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collision)
    {
      
        if (collision.gameObject.tag == "Food")
        {

            itemText.text = "Press E to Eat";
            itemText.enabled = true;

        } else if (collision.gameObject.tag == "Health")
        {

            itemText.text = "Press E to Use";
            itemText.enabled = true;

        } else if (collision.gameObject.tag == "Plank")
        {

            itemText.text = "Press E to Collect";
            itemText.enabled = true;

        }
    }

    void OnTriggerExit(Collider collision)
    {
       
        if (collision.gameObject.tag == "Food" || collision.gameObject.tag == "Health" || collision.gameObject.tag == "Plank")
        {
            itemText.enabled = false;
        }
    }
}

