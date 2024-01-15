using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class lose : MonoBehaviour
{
    public Button tryAgain;
    public Button restart;

    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music.Play();
        tryAgain.onClick.AddListener(buttonClick1);
        restart.onClick.AddListener(buttonClick2);
    }

    void buttonClick1()
    {
        SceneManager.LoadScene("game");
    }

    void buttonClick2()
    {
        SceneManager.LoadScene("begin");
    }
}
