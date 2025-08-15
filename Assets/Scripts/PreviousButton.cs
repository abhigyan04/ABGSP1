using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviousButton : MonoBehaviour
{
    public Button previous;
    public Button next;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public AudioSource buttonClickSound;

    private void Start()
    {
        previous.interactable = false;
    }

    public void OnButtonPress()
    {
        if (panel2.activeInHierarchy)
        {
            panel1.SetActive(true);
            panel2.SetActive(false);
            panel3.SetActive(false);
            previous.interactable = false;
            next.interactable = true;
        }
        else if (panel3.activeInHierarchy)
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
            panel3.SetActive(false);
            next.interactable = true;
            previous.interactable = true;
        }
    }

    public void PlaySound()
    {
        buttonClickSound.Play();
    }
}
