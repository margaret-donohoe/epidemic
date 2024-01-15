using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dice : MonoBehaviour
{
    public GameObject d6;
    public GameObject d8;
    //public bool isSpinning = false;
    public Animator sixAnim;
    public Animator eightAnim;

    public Sprite eight1;
    public Sprite eight2;
    public Sprite eight3;
    public Sprite eight4;
    public Sprite eight5;
    public Sprite eight6;
    public Sprite eight7;
    public Sprite eight8;


    public Sprite six1;
    public Sprite six2;
    public Sprite six3;
    public Sprite six4;
    public Sprite six5;
    public Sprite six6;

    private static System.Random diceRoller = new System.Random(System.DateTime.Now.Second);

    Dictionary<string, Sprite> place = new Dictionary<string, Sprite>();
    Dictionary<string, Sprite> takeReplace = new Dictionary<string, Sprite>();

    private int d6value;
    private int d8value;


    // Start is called before the first frame update
    void Start()
    {
        takeReplace.Add("six1", six1);
        takeReplace.Add("six2", six2);
        takeReplace.Add("six3", six3);
        takeReplace.Add("six4", six4);
        takeReplace.Add("six5", six5);
        takeReplace.Add("six6", six6);
        place.Add("eight1", eight1);
        place.Add("eight2", eight2);
        place.Add("eight3", eight3);
        place.Add("eight4", eight4);
        place.Add("eight5", eight5);
        place.Add("eight6", eight6);
        place.Add("eight7", eight7);
        place.Add("eight8", eight8);

        //sixAnim = GetComponent<Animator>();
        //eightAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int[] roll()
    {
        int[] values = new int[2];
        d6value = diceRoller.Next(6) + 1;
        d8value = diceRoller.Next(8) + 1;
        values[0] = d8value;
        values[1] = d6value;
        StartCoroutine(rollTime());
        return values;
    }

    string setSpriteName(int rollValue, string slot)
    {
        string num = rollValue.ToString();
        string sprite = slot + num;
        return sprite;
    }

    public int coinFlip(float time)
    {
        int num = diceRoller.Next(2) + 1;
        return num;

    }

    IEnumerator rollTime()
    {
        yield return StartCoroutine(rolling());
    }

    IEnumerator rolling()
    {
        sixAnim.SetBool("isSpinning", true);
        eightAnim.SetBool("isSpinning", true); yield return new WaitForSeconds(2);
        string sixName = setSpriteName(d6value, "six");
        string eightName = setSpriteName(d8value, "eight");
        d6.GetComponent<SpriteRenderer>().sprite = assignSprite(sixName, takeReplace);
        d8.GetComponent<SpriteRenderer>().sprite = assignSprite(eightName, place);
        sixAnim.SetBool("isSpinning", false);
        eightAnim.SetBool("isSpinning", false);
        yield return new WaitForSeconds(1);
    }

    Sprite assignSprite (string spriteName, Dictionary<string, Sprite> slots)
    {
        Sprite assignment = null;
        foreach(var slot in slots)
        {
            string name = slot.Key;
            if(name == spriteName)
            { 
                assignment = slot.Value;
            }
            
        }
        //Debug.Log(assignment);
        return assignment;
    }
}
