using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class playerInput : MonoBehaviour
{
    public Button rollButton;
    public Button submitButton;

    private string pieceColor;

    public Sprite redGerm;
    public Sprite redVirus;
    public Sprite redInfection;
    public Sprite redAntibody;
    public Sprite blueGerm;
    public Sprite blueVirus;
    public Sprite blueInfection;
    public Sprite blueAntibody;
    public Sprite yellowGerm;
    public Sprite yellowVirus;
    public Sprite yellowInfection;
    public Sprite yellowAntibody;
    public Sprite greenGerm;
    public Sprite greenVirus;
    public Sprite greenInfection;
    public Sprite greenAntibody;
    public Sprite playerCanHrt;
    public Sprite playerCanBlk;

    public cpuInput meelo;
    //public Sprite indicator;

    public GameObject[] heartBoard;
    public GameObject[] regBoard;

    private Sprite currentlyUsing;
   

    public dice slots; //tag as "replace"

    public GameObject germSpace;
    public GameObject virusSpace;
    public GameObject infectionSpace;
    public GameObject antibodySpace;

    List<GameObject> canPlaceHere = new List<GameObject>();

    
    private int pScore;
    //private int eScore;
    
     //scale determined by scoreRatio


    public TextMeshProUGUI playerPrompt;
    public TextMeshProUGUI enemyReactions; //just axclaimations!!! make it simple (!, ..., >:0, etc)
    private string pPrompt;
    private string eReactions;
    private int place;
    private int take;
    private int replace;
    private int takeReplace;
    public GameObject obj = null;
    private bool isBeginning;
    private bool isSecond = false;

    bool isSafe;
    float hexRadius = 1f;
    float adjRadius = .75f;

    // Start is called before the first frame update
    void Start()
    {
        regBoard = GameObject.FindGameObjectsWithTag("regSpace");
        heartBoard = GameObject.FindGameObjectsWithTag("heartSpace");
        rollButton.interactable = false;
        submitButton.interactable = false;
        pieceColor = PlayerPrefs.GetString("color");

        assignRegular();
        assignSpecial();
        assignHeart();
        assignBlocker();

        currentlyUsing = virusSpace.GetComponent<SpriteRenderer>().sprite;
        SpriteRenderer r = virusSpace.GetComponent<SpriteRenderer>();
        currentlyUsing = r.sprite;
        virusSpace.GetComponent<Piece>().click(r, 0, currentlyUsing);

        place = 3;
        string plString = place.ToString();
        playerPrompt.text = ("place " + plString + " viruses.");
        isBeginning = true;

        //in virus clickTime, check isBeginning, if it is viruses can be placed on blank spaces



        rollButton.onClick.AddListener(rollTime);
        submitButton.onClick.AddListener(submit);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.tag != "ignore")
            {
                if (hit.collider.gameObject.tag.Contains("Piece") || hit.collider.gameObject.tag == "regSpace" || hit.collider.gameObject.tag == "heartSpace")
                {
                    obj = hit.collider.gameObject;
                    //sprt = obj.sprite;
                    clickTime(obj);
                }
            }
        }

    }

    void assignRegular()
    {
        if (pieceColor == "red")
        {
            germSpace.GetComponent<SpriteRenderer>().sprite = redGerm;
        }
        if (pieceColor == "blue")
        {
            germSpace.GetComponent<SpriteRenderer>().sprite = blueGerm;
        }
        if (pieceColor == "yellow")
        {
            germSpace.GetComponent<SpriteRenderer>().sprite = yellowGerm;
        }
        if (pieceColor == "green")
        {
            germSpace.GetComponent<SpriteRenderer>().sprite = greenGerm;
        }
    }

    void assignSpecial()
    {
        if (pieceColor == "red")
        {
            virusSpace.GetComponent<SpriteRenderer>().sprite = redVirus;
        }
        if (pieceColor == "blue")
        {
            virusSpace.GetComponent<SpriteRenderer>().sprite = blueVirus;
        }
        if (pieceColor == "yellow")
        {
            virusSpace.GetComponent<SpriteRenderer>().sprite = yellowVirus;
        }
        if (pieceColor == "green")
        {
            virusSpace.GetComponent<SpriteRenderer>().sprite = greenVirus;
        }
    }

    void assignHeart()
    {
        
        if (pieceColor == "red")
        {
            infectionSpace.GetComponent<SpriteRenderer>().sprite = redInfection;
        }
        if (pieceColor == "blue")
        {
            infectionSpace.GetComponent<SpriteRenderer>().sprite = blueInfection;
        }
        if (pieceColor == "yellow")
        {
            infectionSpace.GetComponent<SpriteRenderer>().sprite = yellowInfection;
        }
        if (pieceColor == "green")
        {
            infectionSpace.GetComponent<SpriteRenderer>().sprite = greenInfection;
        }
    }

    void assignBlocker()
    {
        if (pieceColor == "red")
        {
            antibodySpace.GetComponent<SpriteRenderer>().sprite = redAntibody;
        }
        if (pieceColor == "blue")
        {
            antibodySpace.GetComponent<SpriteRenderer>().sprite = blueAntibody;
        }
        if (pieceColor == "yellow")
        {
            antibodySpace.GetComponent<SpriteRenderer>().sprite = yellowAntibody;
        }
        if (pieceColor == "green")
        {
            antibodySpace.GetComponent<SpriteRenderer>().sprite = greenAntibody;
        }
    }

    void rollTime()
    {
        StartCoroutine(roll());
    }


    IEnumerator roll () //assigns place and takeReplace to random values gotten from slots script
    {
        rollButton.interactable = false;
        int[] values = slots.roll();
        place = values[0];
        string plString = place.ToString();
        yield return new WaitForSeconds(3);

        List<GameObject> canPL = new List<GameObject>();
        canPL = checkSpaces(regBoard, germSpace.GetComponent<SpriteRenderer>().sprite);
        if(canPL.Count == 0)
        {
            float f = PlayerPrefs.GetFloat("scoreRatio");
            StartCoroutine(endGame(f));
        }

        if (isSecond == false)
        {
            takeReplace = values[1];
            if (takeReplace < 4)
            {
                take = takeReplace;
                replace = 0;
                string trString = takeReplace.ToString();
                playerPrompt.text = "place: " + plString + "\ntake: " + trString + "\nclick on enemy germs with VIRUS selected to take them.";
            }
            else
            {
                replace = takeReplace - 3;
                take = 0;
                string trString = replace.ToString();
                playerPrompt.text = "place: " + plString + "\nreplace: " + trString + "\nselect germ to replace enemy germs, or virus to replace your own.";
            }
        }

        else if(isSecond == true)
        {
            take = 0;
            replace = 0;
            playerPrompt.text = "place: " + plString + " germs. \nclick on a piece in the bottom left corner to select it.";
        }
        
    }

    

    void submit() //activation/deactivation of submit button
    {
        submitButton.interactable = false;
        rollButton.interactable = false;
        if (isSecond == true)
        {
            isSecond = false;
        }

        if (isBeginning == true)
        {
            isBeginning = false;
            isSecond = true;
        }

        playerPrompt.text = "wait your turn.";

        StartCoroutine(meelo.Play());
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="g"></param>

    public void clickTime(GameObject g)  //RECONFIGURE THIS!!!
    {
        SpriteRenderer sr = g.GetComponent<SpriteRenderer>();


        if (g.tag == "regPiece" || g.tag == "spePiece" || g.tag == "hrtPiece")
        {
            currentlyUsing = sr.sprite;
            g.GetComponent<Piece>().click(sr, 0, currentlyUsing);
        }

        if (g.tag == "blkPiece")
        {
            currentlyUsing = sr.sprite;
            g.GetComponent<Piece>().click(sr, 0, currentlyUsing);
            List<GameObject> canBLK = new List<GameObject>();
            canBLK = checkSpaces(heartBoard, playerCanBlk);
            foreach (GameObject obj in canBLK)
            {
                obj.GetComponent<SpriteRenderer>().sprite = playerCanBlk;
            }
        }

            if (g.tag == "regSpace" || g.tag == "heartSpace")
        {

            if(currentlyUsing.name.Contains("Germ"))
            {
                //replace enemy germ
                if (isFull(sr) == true && replace > 0) 
                {
                    if(checkLegality(currentlyUsing, sr) == true)
                    {
                        g.AddComponent<Piece>().click(sr, 10, currentlyUsing);
                        subtractReplace();
                        if (playerPrompt.text != null)
                        {
                            string plString = place.ToString();
                            string trString = replace.ToString();
                            playerPrompt.text = "place: " + plString + "\nreplace: " + trString;
                        }
                    }
                }

                //place self germ. checkLegality not needed
                if (isFull(sr) == false) 
                {
                    if (checkAdjacent(sr) == true && place > 0)
                    {
                        g.AddComponent<Piece>().click(sr, 1, currentlyUsing);
                        subtractPlace();

                        if (playerPrompt.text != null)
                        {
                            if(take > 0)
                            {
                                string plString = place.ToString();
                                string trString = take.ToString();
                                playerPrompt.text = "place: " + plString + "\ntake: " + trString;
                            }
                            if(replace > 0)
                            {
                                string plString = place.ToString();
                                string trString = replace.ToString();
                                playerPrompt.text = "place: " + plString + "\nreplace: " + trString;
                            }
                            else if (take == 0 && replace == 0)
                            {
                                string plString = place.ToString();
                                playerPrompt.text = "place: " + plString + " germs.";
                            }
                        }

                        List<GameObject> canPL = new List<GameObject>();
                        canPL = checkSpaces(heartBoard, playerCanHrt);
                        foreach (GameObject obj in canPL)
                        {
                            obj.GetComponent<SpriteRenderer>().sprite = playerCanHrt;
                        }
                        canPlaceHere.Clear();
                    }
                }
            }

                //replace self germ
                if (currentlyUsing.name.Contains("Virus")) 
            {
                if(isBeginning == false)
                {
                        if (isFull(sr) == true && take > 0)
                        {
                            if (checkLegality(currentlyUsing, sr) == true)
                            {
                                g.AddComponent<Piece>().click(sr, 2, currentlyUsing);
                                subtractTake();
                                if (playerPrompt.text != null)
                                {
                                    string plString = place.ToString();
                                    string trString = take.ToString();
                                    playerPrompt.text = "place: " + plString + "\ntake: " + trString;
                                }
                            }
                        }

                    if (isFull(sr) == true && replace > 0)
                    {
                        if (checkLegality(currentlyUsing, sr) == true)
                        {
                            g.AddComponent<Piece>().click(sr, 2, currentlyUsing);
                            subtractReplace();
                            if (playerPrompt.text != null)
                            {
                                string plString = place.ToString();
                                string trString = replace.ToString();
                                playerPrompt.text = "place: " + plString + "\nreplace: " + trString;
                            }
                        }
                    }
                }

            }

            if (currentlyUsing.name.Contains("Virus"))
            {
                if (isFull(sr) == false && place > 0)
                {
                    if (isBeginning == true)
                    {
                        g.AddComponent<Piece>().click(sr, 9, currentlyUsing);
                        subtractPlace();
                        string plString = place.ToString();
                        playerPrompt.text = ("place " + plString + " viruses.");
                    }
                }

                if(place == 0 && isBeginning == true)
                {
                    submitButton.interactable = true;
                    playerPrompt.text = ("submit!");
                }
            }

            if (currentlyUsing.name.Contains("Infection"))//place self infection
            {
                if (sr.sprite == playerCanHrt)
                {
                    g.AddComponent<Piece>().click(sr, 3, currentlyUsing);
                }
            }

            if (currentlyUsing.name.Contains("Antibody"))//replace enemy infection
            {
                if (sr.sprite == playerCanBlk)
                {
                    g.AddComponent<Piece>().click(sr, 4, currentlyUsing);
                }
            }

        }

    }

    public bool checkLegality(Sprite s, SpriteRenderer sr)
    {
        //ONLY USE WHEN SR.SPRITE != NULL!
        if(s.name.Contains("Germ") && sr.sprite.name.Contains("enemyGerm")) //replace enemy germ
        {
                return true;
        }

        if (s.name.Contains("Virus") && sr.sprite.name.Contains("enemy") == false)
        {
            if (sr.sprite.name.Contains("Germ") == true && replace > 0) //replace self germ
            {
                return true;
            }
        }
        if (s.name.Contains("Virus") && sr.sprite.name.Contains("enemyGerm") == true) //take enemy germ
        {
                return true;
        }
 
        else
        {
            return false;
        }
        //regular: check if at least 1 full renderer's sprite name on layer 6 within radius contains pieceColor AND if (isFull(spriteRenderer) == false)
        //special: if (isFull(spriteRenderer) == true && spriteRenderer.tag == "regPiece"
        //heart: sr.sprite == playerCanHrt
        //blocker: check if spriteRenderer sprite name contains enemy, and if 3 renderers on layer 6 within hex (larger radius) are full with pieces that have sprite names containing pieceColor
    }

    public List<GameObject> checkSpaces(GameObject[] spaces, Sprite s) //JUST HEARTS for player, add check for regSpaces or heartSpaces for cpu
    {
        List<GameObject> canPlace = new List<GameObject>();
        if (s == playerCanHrt)
        {
            foreach (GameObject space in spaces)
            {
                Vector2 point = space.transform.position;
                SpriteRenderer spr = space.GetComponent<SpriteRenderer>();
                

                if (isFull(spr) == false)
                {
                    if (checkHex(point, 6) == true)
                    {
                        canPlace.Add(space);
                    }
                }
            }
        }

        if (s == playerCanBlk)
        {
            foreach (GameObject space in spaces)
            {
                Vector2 point = space.transform.position;

                if (space.GetComponent<SpriteRenderer>().sprite != null)
                {
                    Sprite sp = space.GetComponent<SpriteRenderer>().sprite;

                    if (checkHex(point, 3) == true && sp.name == "enemyInfection")
                    {
                        canPlace.Add(space);
                    }
                }
            }
        }

        if(s.name.Contains("Germ"))
        {
            foreach (GameObject space in spaces)
            {
                if (space.GetComponent<SpriteRenderer>().sprite == null)
                {
                    SpriteRenderer spr = space.GetComponent<SpriteRenderer>();
                    if(checkAdjacent(spr) == true && isFull(spr) == false)
                    {
                        canPlace.Add(space);
                    }
                }
            }

        }
        return canPlace;
    }

    IEnumerator endGame(float s)
    {
        playerPrompt.text = "you can no longer place germs. the game is over.";
        yield return new WaitForSeconds(2);
        
        if (s >= 0.5)
        {
            SceneManager.LoadScene("lose");
        }
        else
        {
            SceneManager.LoadScene("win");
        }

    }

    bool isFull(SpriteRenderer check)
    {
        if (check.sprite == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool checkHex(Vector2 location, int i)
    {
        Collider2D[] surroundingSpaces = Physics2D.OverlapCircleAll(location, hexRadius, 1 << 6);

        int filledWithSelf = 0;
        foreach(Collider2D space in surroundingSpaces)
        {
            Sprite s = space.gameObject.GetComponent<SpriteRenderer>().sprite;
            if (s != null)
            {
                if (s.name.Contains(pieceColor))
                { 
                    filledWithSelf++;
                }
            }
        }
        if (filledWithSelf >= i)
        {
            return true;
        }
        else
            return false;
    }

    bool checkAdjacent(SpriteRenderer regSp)
    {
        Vector2 point = regSp.transform.position;
        int filledWithSelf = 0;
        Collider2D[] surroundingSpaces = Physics2D.OverlapCircleAll(point, adjRadius);

        foreach (Collider2D space in surroundingSpaces)
        {
            Sprite s = space.gameObject.GetComponent<SpriteRenderer>().sprite;
            if (s != null)
            {
                if (s.name.Contains(pieceColor))
                {
                    filledWithSelf++;
                }
            }
        }
        if (filledWithSelf > 0)
        {
            return true;
        }

        else
            return false;
    }

    public void subtractPlace()
    {
        place--;
    }//SUBTRACTS FROM # OF GERMS ABLE TO BE PLACED

    public void subtractTake()
    {
        take--;
    }
    public void subtractReplace()
    {
        
        replace--;
    }
}

