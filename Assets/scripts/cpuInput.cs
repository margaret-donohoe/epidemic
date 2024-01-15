using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class cpuInput : MonoBehaviour
{

    public Sprite germ;
    public Sprite virus;
    public Sprite infection;
    public Sprite antibody;

    public GameObject regularSpace;
    public GameObject specialSpace;
    public GameObject heartSpace;
    public GameObject blockerSpace;

    public Button rollB;
    public Button submit;

    public GameObject[] heartBoard;
    public GameObject[] regBoard;
    private SpriteRenderer psr;
    public dice slots;

    private Sprite currentlyUsing;
    private SpriteRenderer sr;
    private int place;
    private int takeReplace;
    private GameObject obj = null;
    public TextMeshProUGUI playerPrompt;
    public TextMeshProUGUI enemyReactions;
    private const float pauseBtwnSelections = 1f;
    private const float pauseBtwnPlacements = 0.5f;
    private int take;
    private int replace;
    private bool isReplace;

    private string pieceColor;

    private int turnStage = 0;
    private string e = "enemy";
    List<GameObject> canPlaceHere = new List<GameObject>();
    List<GameObject> placeDown = new List<GameObject>();

    const float hexRadius = 1f;
    const float adjRadius = .75f;

    private bool isBeginning = true;
    private bool isSecond = false;
    private int filledWithSelf;

    // Start is called before the first frame update
    void Start()
    {
        regBoard = GameObject.FindGameObjectsWithTag("regSpace");
        heartBoard = GameObject.FindGameObjectsWithTag("heartSpace");
        pieceColor = PlayerPrefs.GetString("color");

        canPlaceHere.Clear();
        placeDown.Clear();
        isBeginning = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Play()
    {
        //playerPrompt.text = "wait your turn.";
        
        regBoard = GameObject.FindGameObjectsWithTag("regSpace");
        heartBoard = GameObject.FindGameObjectsWithTag("heartSpace");
        enemyReactions.text = "...";
        if (isBeginning == false)
        {
            int[] values = slots.roll();

            yield return new WaitForSeconds(3f);
            place = values[0];

            if (isSecond == false)
            {
                isBeginning = false;
                takeReplace = values[1];
                if (takeReplace < 4)
                {
                    take = takeReplace;
                    replace = 0;
                }
                else
                {
                    replace = takeReplace - 3;
                    take = 0;
                }
                yield return StartCoroutine(germSelect(regularSpace));
                enemyReactions.text = "!";

                yield return new WaitForSeconds(pauseBtwnSelections);

                yield return StartCoroutine(virusSelect(specialSpace));

                yield return new WaitForSeconds(pauseBtwnSelections);
                yield return StartCoroutine(infectionSelect(heartSpace));

                yield return new WaitForSeconds(pauseBtwnSelections);
                yield return StartCoroutine(antibodySelect(blockerSpace));
            }
            

            if (isSecond == true)
            {
                takeReplace = 0;
                take = 0;
                replace = 0;
                enemyReactions.text = "!";
                yield return StartCoroutine(germSelect(regularSpace));
                yield return new WaitForSeconds(pauseBtwnSelections);

                yield return StartCoroutine(infectionSelect(heartSpace));

                isSecond = false;
                isBeginning = false;
            }
        }

        else if (isBeginning == true)
        {
            enemyReactions.text = "!";
            yield return StartCoroutine(virusSelect(specialSpace));
            isSecond = true;
        }
        enemyReactions.text = ".";
        yield return new WaitForSeconds(0.5f);
        enemyReactions.text = "";
        returnTorch();
    }

    IEnumerator germSelect(GameObject selection)
    {
        sr = selection.GetComponent<SpriteRenderer>();
        currentlyUsing = sr.sprite;
        selection.GetComponent<Piece>().click(sr, 0, currentlyUsing);
        placeDown = checkSpaces(regBoard, currentlyUsing, false);

        if(placeDown.Count == 0)
        {
            float f = PlayerPrefs.GetFloat("scoreRatio");
            StartCoroutine(endGame(f));
        }
        //place germs
        
        foreach (GameObject space in placeDown)
        {
            if (place > 0)
            {
                psr = space.GetComponent<SpriteRenderer>();
                space.AddComponent<Piece>().click(psr, 5, currentlyUsing);
                yield return new WaitForSeconds(pauseBtwnPlacements);
                place--; 
            }
            else if( place > 0 && placeDown.Count == 0)
            {
                List <GameObject> again = checkSpaces(regBoard, currentlyUsing, false);
                foreach (GameObject p in again)
                {
                    if (place > 0)
                    {
                        psr = p.GetComponent<SpriteRenderer>();
                        p.AddComponent<Piece>().click(psr, 5, currentlyUsing);
                        yield return new WaitForSeconds(pauseBtwnPlacements);
                        place--;
                    }
                }
            }
            else if(place == 0)
            {
                break;
            }
        }

        //replace enemy germs
        if (replace > 0 && isSecond == false) 
        {
            int number = slots.coinFlip(Time.realtimeSinceStartup);
            placeDown = checkSpaces(regBoard, currentlyUsing, true);
            foreach (GameObject space in placeDown)
            {
                if (number > 0 && replace > 0)
                {
                    psr = space.GetComponent<SpriteRenderer>();
                    space.AddComponent<Piece>().click(psr, 10, currentlyUsing);
                    yield return new WaitForSeconds(pauseBtwnPlacements);
                    number--;
                    replace--;
                }
                else
                    break;
            }
        }

        placeDown.Clear();
        canPlaceHere.Clear();
    }

    IEnumerator virusSelect(GameObject selection)
    {
        sr = selection.GetComponent<SpriteRenderer>();
        currentlyUsing = sr.sprite;
        selection.GetComponent<Piece>().click(sr, 0, currentlyUsing);
        canPlaceHere.Clear();
        //BEGINNING
        if (isBeginning == true)
        {
            canPlaceHere = checkEmpty(regBoard);
            int pl = 3;
            randomize(canPlaceHere);
            Debug.Log(canPlaceHere.Count);
            foreach (GameObject space in canPlaceHere)
            {
                Debug.Log("iterates");
                if (pl > 0)
                {
                    psr = space.GetComponent<SpriteRenderer>();
                    space.AddComponent<Piece>().click(psr, 9, currentlyUsing);
                    Debug.Log("place");
                    pl -= 1 ;
                    
                }
                if (pl == 0)
                    break;
                yield return new WaitForSeconds(pauseBtwnPlacements);
            }
            isBeginning = false;
            isSecond = true;
        }

        //replace self germ
        else if(isSecond == false)
        {
            if (replace > 0)
            {
                Debug.Log("gets to loop2");
                placeDown = checkSpaces(regBoard, currentlyUsing, true);
                foreach (GameObject space in placeDown)
                {
                    if (replace > 0)
                    {
                        psr = space.GetComponent<SpriteRenderer>();
                        space.AddComponent<Piece>().click(psr, 6, currentlyUsing);
                        yield return new WaitForSeconds(pauseBtwnPlacements);
                        replace--;
                    }
                    else
                        break;
                }
            }

            //take germ
            if (take > 0)
            {
                placeDown = checkSpaces(regBoard, currentlyUsing, false);
                foreach (GameObject space in placeDown)
                {
                    if (take > 0)
                    {
                        psr = space.GetComponent<SpriteRenderer>();
                        space.AddComponent<Piece>().click(psr, 6, currentlyUsing);
                        yield return new WaitForSeconds(pauseBtwnPlacements);
                        take--;
                    }
                    else
                        break;
                }
            }
        }

        placeDown.Clear();
        canPlaceHere.Clear();

    }

    IEnumerator infectionSelect(GameObject selection) //finds and places infections
    {
        sr = selection.GetComponent<SpriteRenderer>();
        currentlyUsing = sr.sprite;
        selection.GetComponent<Piece>().click(sr, 0, currentlyUsing);
        placeDown = checkSpaces(heartBoard, currentlyUsing, false);

        foreach(GameObject space in placeDown)
        {
            psr = space.GetComponent<SpriteRenderer>();
            space.AddComponent<Piece>().click(psr, 7, currentlyUsing);
            yield return new WaitForSeconds(pauseBtwnPlacements);
        }
        placeDown.Clear();
        canPlaceHere.Clear();
    }

    IEnumerator antibodySelect(GameObject selection) //finds and places antibodies
    {
        sr = selection.GetComponent<SpriteRenderer>();
        currentlyUsing = sr.sprite;
        selection.GetComponent<Piece>().click(sr, 0, currentlyUsing);
        placeDown = checkSpaces(heartBoard, currentlyUsing, false);

        foreach (GameObject space in placeDown)
        {
            psr = space.GetComponent<SpriteRenderer>();
            space.AddComponent<Piece>().click(psr, 8, currentlyUsing);
            yield return new WaitForSeconds(pauseBtwnPlacements);
        }
        placeDown.Clear();
        canPlaceHere.Clear();
    }

    void returnTorch()
    {
        submit.interactable = true;
        rollB.interactable = true;
        playerPrompt.text = "roll.";
    }

    IEnumerator endGame(float s)
    {
        playerPrompt.text = "meelo can no longer place germs. the game is over.";
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

    bool checkHex(Vector2 location, int i)
    {
        Collider2D[] surroundingSpaces = Physics2D.OverlapCircleAll(location, hexRadius, 1 << 6);

        filledWithSelf = 0;

        if (i != 2)
        {
            foreach (Collider2D space in surroundingSpaces)
            {
                Sprite s = space.gameObject.GetComponent<SpriteRenderer>().sprite;
                if (s != null)
                {
                    if (s.name.Contains(e))
                    {
                        filledWithSelf++;
                    }
                }
            }

            if (filledWithSelf >= i)
            {
                filledWithSelf = 0;
                return true;
            }

            else
            {
                filledWithSelf = 0;
                return false;
            }
        }

        if (i == 2)
        {
            foreach (Collider2D space in surroundingSpaces)
            {
                Sprite s = space.gameObject.GetComponent<SpriteRenderer>().sprite;
                if (s != null)
                {
                    if (s.name.Contains(e))
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
            {
                return false;
            }
        }
        else
            return false;
            
    }

    bool checkAdjacent(SpriteRenderer regSp)
    {
        Vector2 point = regSp.transform.position;
        filledWithSelf = 0;
        Collider2D[] surroundingSpaces = Physics2D.OverlapCircleAll(point, adjRadius, 1 << 6);

        foreach (Collider2D space in surroundingSpaces)
        {
            
            if (space.gameObject.GetComponent<SpriteRenderer>().sprite != null)
            {
                Sprite s = space.gameObject.GetComponent<SpriteRenderer>().sprite;
                if (s.name.Contains(e))
                {
                    filledWithSelf++;
                }
            }
        }

        if (filledWithSelf > 0)
        {
            filledWithSelf = 0;
            return true;
        }
        else
            return false;
    }

    bool checkHearts(SpriteRenderer regSp) //determines if there is a player heart nearby
    {
        Vector2 point = regSp.transform.position;
        int filledWithPlayer = 0;
        Collider2D[] surroundingSpaces = Physics2D.OverlapCircleAll(point, 0.5f, 1 << 7);

        foreach (Collider2D space in surroundingSpaces)
        {
            Sprite s = space.gameObject.GetComponent<SpriteRenderer>().sprite;

            if (s != null)
            {
                if (s.name.Contains(pieceColor) && s.name.Contains("Infection"))
                {
                    filledWithPlayer++;
                }
            }
        }

        if (filledWithPlayer >= 1)
        {
            //Debug.Log(filledWithPlayer);
            return true;
        }
        else
            return false;

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

    public List <GameObject> checkEmpty(GameObject[] spaces)
    {
        canPlaceHere.Clear();
        foreach(GameObject space in spaces)
        {
            SpriteRenderer r = space.GetComponent<SpriteRenderer>();
            if(isFull(r) == false)
            {
                canPlaceHere.Add(space);
            }
        }
        randomize(canPlaceHere);
        return canPlaceHere;

    }

    public List<GameObject> checkSpaces(GameObject[] spaces, Sprite s, bool ir) //JUST HEARTS for player, add check for regSpaces or heartSpaces for cpu
    {
        isReplace = ir;
        canPlaceHere.Clear();

        if(s == germ && ir == false)//place germ. RANDOMIZE. eventually make a method that ranks by advantageousness
        {
            foreach(GameObject space in spaces)
            {
                SpriteRenderer spr = space.GetComponent<SpriteRenderer>();
                if(isFull(spr) == false && checkAdjacent(spr) == true)
                {
                    canPlaceHere.Add(space);
                }
            }
            sort(canPlaceHere);
        }

        else if (s == germ && ir == true)//replace player germ. RANDOMIZE
        {
            foreach (GameObject space in spaces)
            {
                SpriteRenderer spr = space.GetComponent<SpriteRenderer>();
                if (spr.sprite != null)
                {
                    string name = spr.sprite.name;
                    if (isFull(spr) == true && name.Contains("Germ") && name.Contains(pieceColor))
                    {
                        canPlaceHere.Add(space);
                    }
                }
            }
            sort(canPlaceHere);
        }

        if (s == virus && ir == false)//take player germ. RANDOMIZE
        {
            foreach (GameObject space in spaces)
            {
                SpriteRenderer spr = space.GetComponent<SpriteRenderer>();
                if (spr.sprite != null)
                {
                    string name = spr.sprite.name;
                    if (name.Contains("Germ") && name.Contains(pieceColor))
                    {
                        canPlaceHere.Add(space);
                    }
                }
            }
            sort(canPlaceHere);
        }
        else if (s == virus && ir == true)//replace self germ. RANDOMIZE
        {
            foreach (GameObject space in spaces)
            {
                SpriteRenderer spr = space.GetComponent<SpriteRenderer>();
                if (spr.sprite != null)
                {
                    if (spr.sprite == germ)
                    {
                        canPlaceHere.Add(space);
                    }
                }
            }
            randomize(canPlaceHere);
        }

        if (s == infection)
        {
            foreach (GameObject space in spaces)
            {
                Vector2 point = space.transform.position;
                SpriteRenderer spr = space.GetComponent<SpriteRenderer>();

                if (isFull(spr) == false)
                {
                    if (checkHex(point, 6) == true)
                    {
                        canPlaceHere.Add(space);
                        
                    }
                }
            }
        }

        if (s == antibody)
        {
            foreach (GameObject space in spaces)
            {
                Vector2 point = space.transform.position;
                SpriteRenderer spr = space.GetComponent<SpriteRenderer>();
                if (spr.sprite != null)
                {
                    Sprite sp = spr.sprite;
                    if (checkHex(point, 3) == true && sp.name.Contains("Infection") && sp.name.Contains(pieceColor))
                    {
                        canPlaceHere.Add(space);
                    }
                }
            }
        }
        return canPlaceHere;
    }

    public List<GameObject> sort(List<GameObject> placeable) //TO REPLACE RANDOMIZE
    {
        Sprite test = placeable[0].GetComponent<SpriteRenderer>().sprite;
        List<GameObject> s = new List<GameObject>();

        if (test == null) //germ placement, checks for heartable hexes
        {
            foreach (GameObject space in placeable)
            {
                //SpriteRenderer sprt = space.GetComponent<SpriteRenderer>();
                Vector2 point = space.transform.position;
                Collider2D[] surroundingHearts = Physics2D.OverlapCircleAll(point, 0.5f, 1 << 7);
                bool heartPossible = false;

                foreach(Collider2D heartSP in surroundingHearts)
                {
                    Sprite sp = heartSP.gameObject.GetComponent<SpriteRenderer>().sprite;
                    if (sp == null)
                    { 
                        if (checkHex(point, 2) == true)
                        {
                            heartPossible = true;
                        }
                    }
                }

                if (heartPossible == true)
                {
                    s.Insert(0, space);
                }
                else
                {
                    s.Add(space);
                }
            }
        }

        else //germ take or replace, checks for player hearts in area
        {
            foreach (GameObject space in placeable)
            {
                SpriteRenderer sprtR = space.GetComponent<SpriteRenderer>();
                if(checkHearts(sprtR) == true)
                {
                    s.Insert(0, space);
                }
                else
                {
                    s.Add(space);
                }
            }
        }
        Debug.Log(s.Count);
        return s;
    }

    public List<GameObject> randomize(List<GameObject> placeable)
    {
        var count = placeable.Count;
        var last = count - 1;
        float sd = Time.realtimeSinceStartup;
        int seed = (int)(10 * sd);
        Random.InitState(seed);
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = placeable[i];
            placeable[i] = placeable[r];
            placeable[r] = tmp;
        }
        return placeable;
    }
}

