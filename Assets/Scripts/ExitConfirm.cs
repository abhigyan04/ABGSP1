using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitConfirm : MonoBehaviour
{
    public GameObject popupPanel;
    public GameObject blackout;
    public AudioSource buttonClickSound;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButtonPressed();
        }
    }

    private void OnBackButtonPressed()
    {
        if (!popupPanel.activeInHierarchy)
        {
            popupPanel.SetActive(true);
            blackout.SetActive(true);
        }
        else
        {
            popupPanel.SetActive(false);
            blackout.SetActive(false);
        }
    }

    public void ShowPopup()
    {
        if (!popupPanel.activeInHierarchy)
        {
            popupPanel.SetActive(true);
            blackout.SetActive(true);
            Debug.Log("show panel");
        }
        else
        {
            popupPanel.SetActive(false);
            blackout.SetActive(false);
            Debug.Log("hide panel");
        }
    }

    public void HidePopup()
    {
        if ((popupPanel.activeInHierarchy))
        {
            popupPanel.SetActive(false);
            blackout.SetActive(false);
        }
    }

    public void OnYesButton()
    {
        Debug.Log("Exiting game...");
        // Save any game state here if needed
        // Application.Quit(); // Use this if it's a standalone build
        SceneManager.LoadScene(0); // Assuming you have a scene named "HomeScene" for home
    }

    public void OnNoButton()
    {
        HidePopup();
    }

    public void PlaySound()
    {
        if (buttonClickSound != null && buttonClickSound.clip != null)
        {
            buttonClickSound.Play();
        }
    }
}
