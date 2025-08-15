using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    public float splashDuration = 2f;
    public Animator animator;

    void Start()
    {
        StartCoroutine(ShowSplashScreen());
    }

    private IEnumerator ShowSplashScreen()
    {
        animator.SetTrigger("FadeInTrigger");
        yield return new WaitForSeconds(splashDuration);
        animator.SetTrigger("FadeOutTrigger");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
