using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public float fadeInSpeed;
    public float fadeOutSpeed;

    public GameObject fadeImage;

    public bool busy;
    void Start()
    {
        fadeImage = GameObject.Find("SceneChanger_FadeCanvas");

        Animator anim = fadeImage.GetComponent<Animator>();
        anim.speed = fadeInSpeed;
        anim.PlayInFixedTime("Base Layer.Fade In", 0, 0);
    }

  
    public void ChangeLevel(string targetScene) //Changes scene to whatever scene is passed into the function
    {
        if (busy)
            return;
        StartCoroutine( FadeOut(targetScene));
    }

    public IEnumerator FadeOut(string _levelName)
    {
        busy = true;
        Animator anim = fadeImage.GetComponent<Animator>();
        anim.speed = fadeOutSpeed;
        anim.PlayInFixedTime("Base Layer.Fade Out", 0, 0);

        float fadeTimeInSeconds = anim.GetCurrentAnimatorStateInfo(0).length / anim.speed;

        yield return new WaitForSecondsRealtime(fadeTimeInSeconds);
        SceneManager.LoadScene(_levelName);
        busy = false;
    }

}