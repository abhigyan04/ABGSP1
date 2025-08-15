using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    public Button home;
    public AudioSource buttonClickSound;

    public void OnButtonPress()
    {
        SceneManager.LoadScene(0);
    }

    public void PlaySound()
    {
        buttonClickSound.Play();
    }
}
