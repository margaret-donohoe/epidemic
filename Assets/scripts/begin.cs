using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class begin : MonoBehaviour
{
    public Button button;

    public Button chooseRed;
    public Button chooseBlue;
    public Button chooseYellow;
    public Button chooseGreen;

    public Button instructions;
    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music.Play();
        instructions.onClick.AddListener(goDirections);
        button.onClick.AddListener(buttonClick);
        chooseRed.onClick.AddListener(red);
        chooseBlue.onClick.AddListener(blue);
        chooseYellow.onClick.AddListener(yellow);
        chooseGreen.onClick.AddListener(green);
        

        PlayerPrefs.SetString("color", "red");
    }

    void buttonClick()
    {
        SceneManager.LoadScene("game");
    }

    void goDirections()
    {
        Debug.Log("press");
        SceneManager.LoadScene("directions");
    }

    void red()
    {
        PlayerPrefs.SetString("color", "red");
    }

    void blue()
    {
        PlayerPrefs.SetString("color", "blue");
    }

    void yellow()
    {
        PlayerPrefs.SetString("color", "yellow");
    }

    void green()
    {
        PlayerPrefs.SetString("color", "green");
    }
    
}
