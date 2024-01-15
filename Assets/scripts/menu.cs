using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class menu : MonoBehaviour
{

    public Button menuButton;
    public Button backBtn;
    public GameObject menuCanvas;
    public GameObject directions;

    public Button home;
    public Button quit;

    // Start is called before the first frame update
    void Start()
    {
        menuCanvas.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);
        menuButton.onClick.AddListener(click);
        backBtn.onClick.AddListener(back);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void click()
    {
        Time.timeScale = 0f;
        backBtn.gameObject.SetActive(true);
        menuCanvas.gameObject.SetActive(true);

        home.onClick.AddListener(goHome);
        quit.onClick.AddListener(quitTime);

    }

    void back()
    {
        Time.timeScale = 1.0f;
        menuButton.gameObject.SetActive(true);
        menuButton.onClick.AddListener(click);
        backBtn.gameObject.SetActive(false);
        
        menuCanvas.gameObject.SetActive(false);
    }


    void goHome()
    {
        SceneManager.LoadScene("begin");
    }

    void quitTime()
    {
        Application.Quit();
    }
}


