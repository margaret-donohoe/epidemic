using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class board : MonoBehaviour
{
    private GameObject[] heartBoard;
    private GameObject[] regBoard;

    private SpriteRenderer SpriteRenderer;
    public int pScore = 0;
    public int eScore = 0;
    float scoreRatio;
    public bool boardIsFull = false;

    private string e = "enemy";
    private Piece usingC;
    string g = "Germ";
    private string v = "Virus";
    private string i = "Infection";
    private string a = "Antibody";

    public GameObject scoreBG;
    public Image enemyHealthBar;
    private string pieceColor;

    public Sprite greenBar;
    public Sprite blueBar;
    public Sprite redBar;
    public Sprite yellowBar;

    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI enemyScore;

    public AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        music.Play();
        pieceColor = PlayerPrefs.GetString("color");
        regBoard = GameObject.FindGameObjectsWithTag("regSpace");
        heartBoard = GameObject.FindGameObjectsWithTag("heartSpace");
        assignBar(pieceColor);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        checkBoard(regBoard);

        int[] scores = getScores();
        pScore = scores[0];
        eScore = scores[1];
        //Debug.Log(pScore + ", " + eScore);
        if (pScore !=0 && eScore != 0)
        {
            scoreRatio = (float)eScore / (float)(pScore + eScore);
            //Debug.Log(scoreRatio);
            enemyHealthBar.fillAmount = scoreRatio;
            PlayerPrefs.SetFloat("scoreRatio", scoreRatio);
            string p = pScore.ToString();
            string e = eScore.ToString();
            playerScore.text = p;
            enemyScore.text = e;
        }
    }

    void checkBoard(GameObject[] spaces)
    {
        //eventually, this also has to check if there are any hearts or blockers that  can be placed first. possible solution: moving to cpuInput, called after 
        foreach (GameObject space in spaces)
        {
            if (space.GetComponent<SpriteRenderer>().sprite != null)
                boardIsFull = true;
            else
            {
                boardIsFull = false;
                break;
            }
        }


        if (boardIsFull == true)
        {
            endGame(scoreRatio);
        }

    }

    int[] getScores()
    {
        int[] scores = new int[2];
        int pS = 0;
        int eS = 0;
        foreach (GameObject space in regBoard)
        {
            SpriteRenderer sr = space.GetComponent<SpriteRenderer>();
            if (sr.sprite != null)
            {
                string fullName = sr.sprite.name;
                string type = space.GetComponent<Piece>().getType(sr);
                if (type.Contains(g))
                {
                    if (fullName.Contains(e))
                    {
                        eS += 1;
                    }
                    else
                    {
                        pS += 1;
                    }
                }
                if (type.Contains(v))
                {
                    if (fullName.Contains(e))
                    {
                        eS += 3;
                    }
                    else
                    {
                        pS += 3;
                    }
                }
            }
        }
        foreach (GameObject space in heartBoard)
        {
            SpriteRenderer sr = space.GetComponent<SpriteRenderer>();
            if (sr.sprite != null)
            {
                string fullName = sr.sprite.name;
                string type = space.AddComponent<Piece>().getType(sr);
                if (type.Contains(i))
                {
                    if (fullName.Contains(e))
                    {
                        eS += 5;
                    }
                    else
                    {
                        pS += 5;
                    }
                }
                if (type.Contains(a))
                {
                    if (fullName.Contains(e))
                    {
                        eS += 7;
                    }
                    else
                    {
                        pS += 7;
                    }
                }
            }
        }
        scores[0] = pS;
        scores[1] = eS;
        PlayerPrefs.SetInt("pScore", pS);
        PlayerPrefs.SetInt("eScore", eS);
        return scores;
    }

    void endGame(float s)
    {
        if (s >= 0.5)
        {
            SceneManager.LoadScene("lose");
        }
        else
        {
            SceneManager.LoadScene("win");
        }

    }

    void assignBar(string color)
    {
        if (color == "red")
        {
            scoreBG.GetComponent<SpriteRenderer>().sprite = redBar;
        }
        if (color == "blue")
        {
            scoreBG.GetComponent<SpriteRenderer>().sprite = blueBar;
        }
        if (color == "yellow")
        {
            scoreBG.GetComponent<SpriteRenderer>().sprite = yellowBar;
        }
        if (color == "green")
        {
            scoreBG.GetComponent<SpriteRenderer>().sprite = greenBar;
        }
    }
}
