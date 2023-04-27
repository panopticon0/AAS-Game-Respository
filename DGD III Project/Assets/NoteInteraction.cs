using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteInteraction : MonoBehaviour
{

    public TextMeshProUGUI triggerText;
    public GameObject imageCanvas;
    public bool note;
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       if (note == true && Input.GetKeyDown(KeyCode.E))
        {
            imageCanvas.SetActive(true);
            triggerText.enabled = false;
            Cursor.visible = true;
           
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerText.text = "Press E to read";
            triggerText.enabled = true;
            note = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerText.enabled = false;
            note = false;
            if(imageCanvas.activeInHierarchy == false)
            {
                Cursor.visible = false;
            }
        }
    }
}
