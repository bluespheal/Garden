using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastToggler : MonoBehaviour
{
    public GameObject fadeImage;
    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GameObject.Find("SceneChanger_FadeCanvas");
    }

    public void ToggleRaycastTarget()
    {
        print("Toggle raycast");
        fadeImage.GetComponent<Graphic>().raycastTarget = !fadeImage.GetComponent<Graphic>().raycastTarget;
    }

}
