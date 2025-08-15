using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public Button show;
    public TextMeshProUGUI text;
    public AudioSource buttonClick;

    public void OnButtonPress()
    {
        if (text.enabled == false)
        {
            text.enabled = true;
        }
        else
        {
            text.enabled = false;
        }
    }

    public void PlaySound()
    {
        buttonClick.Play();
    }
}
