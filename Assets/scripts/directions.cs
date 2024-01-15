using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class directions : MonoBehaviour
{

    public Button nextSlide;
    int i = 1;

    public Image slide1;
    public Image slide2;
    public Image slide3;

    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music.Play();
        nextSlide.onClick.AddListener(next);
        slide1.enabled = true;
        slide2.enabled = false;
        slide3.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void next()
    {
        if (i == 1)
        {
            slide1.enabled = false;
            slide2.enabled = true;
            i = 2;
            return;
        }

        if (i == 2)
        {
            slide2.enabled = false;
            slide3.enabled = true;
            i = 3;
            return;
        }

        if (i == 3)
        {
            returnHome();
        }
    }

    void returnHome()
    {
        SceneManager.LoadScene("begin");
    }
}
