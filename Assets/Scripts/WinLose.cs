using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinLose : MonoBehaviour
{
    public Button playAgain;
    public Button home;
    public AudioSource buttonClickSound;

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void PlaySound()
    {
        buttonClickSound.Play();
    }
}
