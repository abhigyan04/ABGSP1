using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject settings;
    public GameObject settingsPanel;
    public GameObject blackout;
    public GameObject audioToggle;
    public Sprite minus;
    public Sprite gear;
    public Sprite tick;
    public AudioSource buttonClickSound;
    public AudioSource bgm;


    public void OnSettings()
    {
        if (!settingsPanel.activeInHierarchy)
        {
            settingsPanel.SetActive(true);
            settings.GetComponent<Image>().sprite = minus;
            blackout.SetActive(true);
        }
        else
        {
            settingsPanel.SetActive(false);
            settings.GetComponent<Image>().sprite = gear;
            blackout.SetActive(false);
        }
    }

    public void AudioOnOff()
    {
        if (audioToggle.GetComponent<Image>().sprite == null)
        {
            audioToggle.GetComponent<Image>().sprite = tick;
            bgm.enabled = true;
        }
        else
        {
            audioToggle.GetComponent<Image>().sprite = null;
            bgm.enabled = false;
        }
    }

    public void PlaySound()
    {
        buttonClickSound.Play();
    }
}
