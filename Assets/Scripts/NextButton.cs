using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    public Button next;
    public Button previous;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public AudioSource buttonClickSound;

    public void OnButtonPress()
    {
        if (panel1.activeInHierarchy)
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
            panel3.SetActive(false);
            next.interactable = true;
            previous.interactable = true;
        }
        else if (panel2.activeInHierarchy)
        {
            panel1.SetActive(false);
            panel2.SetActive(false);
            panel3.SetActive(true);
            next.interactable = false;
            previous.interactable = true;
        }
    }

    public void PlaySound()
    {
        buttonClickSound.Play();
    }
}
