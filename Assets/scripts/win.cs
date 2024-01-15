using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class win : MonoBehaviour
{
    public Button restart;
    public Button quit;
    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music.Play();
        restart.onClick.AddListener(buttonClick1);
        quit.onClick.AddListener(buttonClick2);
    }

    void buttonClick1()
    {
        SceneManager.LoadScene("begin");
    }

    void buttonClick2()
    {
        Application.Quit();
    }
}
