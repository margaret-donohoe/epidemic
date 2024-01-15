using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Piece : MonoBehaviour
{
    private string heartSpace = "heartSpace";
    private string regSpace = "regSpace";
    private string regPiece = "regPiece";
    private string spePiece = "spePiece";
    private string hrtPiece = "hrtPiece";
    private string blkPiece = "blkPiece";

    public GameObject germI;
    public GameObject virusI;
    public GameObject infectionI;
    public GameObject antibodyI;

    public Sprite playerCanHrt;
    public Sprite playerCanBlk;

    public Sprite indicator;
    public Sprite pSprite;

    string pName = null;
 
    SpriteRenderer location; 

    string g = "Germ";
    private string v = "Virus";
    private string i = "Infection";
    private string a = "Antibody";

    private string e = "enemy";
    private string p = "player";

    private string pieceColor;
    private string gTakeName = null;

    private AudioSource placeG;
    private AudioSource placeI;
    private AudioSource take;
    private AudioSource replace;
    private AudioSource antibody;

    private ParticleSystem pPlacement;
    private ParticleSystem ePlacement;

    private void Awake()
    {
        placeG = GameObject.Find("plAudio").GetComponent<AudioSource>();
        placeI = GameObject.Find("plInfAudio").GetComponent<AudioSource>();
        take = GameObject.Find("takeAudio").GetComponent<AudioSource>();
        replace = GameObject.Find("replAudio").GetComponent<AudioSource>();
        antibody = GameObject.Find("antiAudio").GetComponent<AudioSource>();

        pPlacement = GameObject.Find("playerE").GetComponent<ParticleSystem>();
        ePlacement = GameObject.Find("enemyE").GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pieceColor = PlayerPrefs.GetString("color");
        PlayerPrefs.SetInt("pScore", 0);
        PlayerPrefs.SetInt("eScore", 0);
        
        pSprite = GameObject.Find("antiAudio").GetComponent<Sprite>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void placePiece(SpriteRenderer l, Sprite s)
    {
        l.sprite = s;

        Vector3 p = l.gameObject.transform.position;

        if(s.name.Contains(e) == true)
        {
            ePlacement.transform.position = p;
            ePlacement.Play();
        }
        else
        {
            pPlacement.transform.position = p;
            pPlacement.Play();
        }
        
        
    }

    //CONTROL ROUND FLOW, DON'T ALLOW SPECIALS AT PLACEMENT PERIOD

    public void click(SpriteRenderer n, int type, Sprite s) //calls placement if
    {
        //ebug.Log("is this even working");
        location = n;
        pSprite = s;
        GameObject o = n.gameObject;
        pName = s.name;
        

        if(location.sprite != null)
        {
            gTakeName = n.sprite.name;
        }
        else
        {
            gTakeName = "";
        }
        


        //assignment of pSprite, gets duplicated onto board on clicks
        if (type == 0)
        {
            //Debug.Log(pName);
            
            assign(o);
        }

        //place/replace germ (checks tag)
        if (type == 1)
        {
            if (o.tag == "regSpace")
            {
                //Debug.Log("hello");
                if (pName.Contains(g))
                {
                    placePiece(location, pSprite);
                    placeG.Play();
                }
            }
        }

        //replace self with virus OR take enemy germ
        if (type == 2)
        {
            if (o.tag == "regSpace")
            {
                //Debug.Log("hello");
                if (gTakeName != "")
                {
                    if (pName.Contains(v) && gTakeName.Contains("Germ") == true)
                    {
                        placePiece(location, pSprite);
                        replace.Play();
                    }

                    if (gTakeName.Contains("enemyGerm") && pName.Contains(v))
                    {
                        location.sprite = null;
                        take.Play();
                    }
                }
            }
        }

        //place heart (checks tag)
        if (type == 3) 
        {
            if (o.gameObject.tag == "heartSpace")
            {
                if (pName.Contains(i))
                {
                    placePiece(location, pSprite);
                    placeI.Play();
                }
            }
        }

        //replace opponent heart with antibody
        if (type == 4)
        {
            if (o.gameObject.tag == "heartSpace")
            {
                if (pName.Contains(a))
                {
                    placePiece(location, pSprite);
                    antibody.Play();
                }
            }
        }
        //ENEMY INPUT!!!

        if (type == 5)
        {
            if (o.tag == "regSpace")
            {
                //Debug.Log("hello");
                if (pName.Contains(g))
                {
                    placePiece(location, pSprite);
                    placeG.Play();
                }
            }
        }

        //replace self with virus OR take enemy germ
        if (type == 6)
        {
            if (o.tag == "regSpace")
            {
               

                if (gTakeName != "")
                {
                    if (pName.Contains(v) && gTakeName.Contains("Germ"))
                    {
                        placePiece(location, pSprite);
                        replace.Play();
                    }

                    if (gTakeName.Contains("Germ") && gTakeName.Contains(e) == false)
                    {
                        location.sprite = null;
                        take.Play();
                    }
                }
            }
        }

        //place heart (checks tag)
        if (type == 7)
        {
            if (o.gameObject.tag == "heartSpace")
            {
                if (pName.Contains(i))
                {
                    placePiece(location, pSprite);
                    placeI.Play();
                }
            }
        }

        //replace opponent heart with antibody
        if (type == 8)
        {
            if (o.gameObject.tag == "heartSpace")
            {
                if (pName.Contains(a))
                {
                    placePiece(location, pSprite);
                    antibody.Play();
                }
            }
        }

        //beinning
        if(type == 9)
        {
            if (o.gameObject.tag == "regSpace")
            {
                placePiece(location, pSprite);
                placeI.Play();
            }
        }

        if (type == 10)//replace germ
        {
            if (o.tag == "regSpace")
            {
                //Debug.Log("hello");
                if (pName.Contains(g))
                {
                    placePiece(location, pSprite);
                    replace.Play();
                }
            }
        }

    }
    public void assign(GameObject click)
    {
        pSprite = click.GetComponent<SpriteRenderer>().sprite;

        if (pSprite.name.Contains("Germ"))
        {
            germI.GetComponent<SpriteRenderer>().sprite = indicator;
            virusI.GetComponent<SpriteRenderer>().sprite = null;
            infectionI.GetComponent<SpriteRenderer>().sprite = null;
            antibodyI.GetComponent<SpriteRenderer>().sprite = null;
        }
        if (pSprite.name.Contains("Virus"))
        {
            virusI.GetComponent<SpriteRenderer>().sprite = indicator;
            germI.GetComponent<SpriteRenderer>().sprite = null;
            infectionI.GetComponent<SpriteRenderer>().sprite = null;
            antibodyI.GetComponent<SpriteRenderer>().sprite = null;
        }
        if (pSprite.name.Contains("Infection"))
        {
            infectionI.GetComponent<SpriteRenderer>().sprite = indicator;
            germI.GetComponent<SpriteRenderer>().sprite = null;
            virusI.GetComponent<SpriteRenderer>().sprite = null;
            antibodyI.GetComponent<SpriteRenderer>().sprite = null;
        }
        if (pSprite.name.Contains("Antibody"))
        {
            antibodyI.GetComponent<SpriteRenderer>().sprite = indicator;
            germI.GetComponent<SpriteRenderer>().sprite = null;
            virusI.GetComponent<SpriteRenderer>().sprite = null;
            infectionI.GetComponent<SpriteRenderer>().sprite = null;

        }
    }

    public string getType(SpriteRenderer s)
    {
        string n = s.sprite.name;
        string t = "";
        if(n.Contains(g))
        {
            t = g;
        }
        if (n.Contains(v))
        {
            t = v;
        }
        if (n.Contains(i))
        {
            t = i;
        }
        if (n.Contains(a))
        {
            t = a;
        }
        return t;
    }
}
