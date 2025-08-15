using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button start;
    public Button easy;
    public Button medium;
    public Button hard;
    public Button howToPlay;
    public Button home;
    public Image S;
    public Image T;
    public Image R;
    public Image T2;
    public AudioSource buttonClickSound;


    public void DifficultyMenu()
    {
        start.gameObject.SetActive(false);
        howToPlay.gameObject.SetActive(false);
        S.enabled = false;
        T.enabled = false;
        R.enabled = false;
        T2.enabled = false;
        GameManager.difficulty = 1;
        SceneManager.LoadScene(1);
    }


    public void HowToPlay()
    {
        SceneManager.LoadScene(2);
    }


    public void Easy()
    {
        GameManager.difficulty = 1;
        SceneManager.LoadScene(1);
    }


    public void Medium()
    {
        GameManager.difficulty = 2;
        SceneManager.LoadScene(1);
    }


    public void Hard()
    {
        GameManager.difficulty = 3;
        SceneManager.LoadScene(1);
    }


    public void Restart()
    {
        Debug.Log("button clicked");
        SceneManager.LoadScene(0);
    }


    public void PlaySound()
    {
        buttonClickSound.Play();
    }
}
