using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChordManager : MonoBehaviour
{


    //Variables



    //Holds the text of the chord, so it can be changed as needed

    public TextMesh chordText;



    //Total count of all targets may change per difficulty
    //int totalTargetNum = 4;


    //To hold the target's GameObjects
    //public List<GameObject> targets;

    //to hold GameManager Instance
    GameManager gm;
    //GameObject gmObj;


    //To hold the corect target index list
    List<int> currentChordLineAndTargetIndexList = new List<int>();
    //the target GameObjects
    string[] targetNames = new string[] { "Target", "Target (1)", "Target (2)", "Target (3)" };


    //Colors to change target to when the correct chord is hit
    //public Color[] hitColors = new Color[4];
    //Normal target colors
    //public Color[] targetColors = new Color[4];
    //Animator for target getting hit
    private Animator targetAnimator;

    //To hold all the Text Compnenets of the Text GameObject under Canvas
    Text txtTargetHit;
    //Text txtChordNameTest;
    Text txtChordCount;
    Text txtWrongCount;
    //Text txtTimeCount;

    Text txtCurrentCorrectChordName;

    //Test variables
    static float t1;
    static float timeDif;
    static int count;

    //Counting the number of correct guesses
    static int correctCount;
    static int wrongCount;


    //Variables to calculate how fast the chords should fall
    float bpm = 120;
    const float sec = 60;
    float bpmOverSec;

    //this will be the y coordinate to place each new chord line.. if i have worked out the correct sec/bpm as per unit in the (x, y) coordinate system for this
    int beat;




    //Start

    // Use this for initialization
    void Start()
    {

        //set chordText to the TextMesh component of a child of this GameObject that this script component is attached to
        chordText = GetComponentInChildren<TextMesh>();

        //set thee bpm/sec for fall speed
        bpmOverSec = bpm / sec;

        //setting each of the Text GameObject's Text Component to a Text variable to be used
        //to alter each durring the game
        txtTargetHit = GameObject.Find("TargetHit").GetComponent<Text>();

        txtChordCount = GameObject.Find("ChordCount").GetComponent<Text>();
        count = 0;
        correctCount = 0;

        txtWrongCount = GameObject.Find("WrongCount").GetComponent<Text>();
        wrongCount = 0;

        //txtTimeCount = GameObject.Find("TimeCount").GetComponent<Text>();

        //txtChordNameTest = GameObject.Find("ChordNameTest").GetComponent<Text>();

        txtCurrentCorrectChordName = GameObject.Find("CurrentCorrectChordName").GetComponent<Text>();


        gm = GameManager.instance;



        //targets.AddRange(gm.getTargets());


        currentChordLineAndTargetIndexList = gm.getCorrectChordLineAndTargetIndexList();


    }





    //MakeChordsFall

    //Method to make chord fall downwards at a certain rate, hopefully close enough to one in-game y-unit per second * sec/bmp
    public void MakeChordFall()
    {
        //Fall at 2y per sec -- Time.deltaTime * 1 would be 1 per sec
        transform.position -= transform.up * (Time.deltaTime * bpmOverSec);

    }











    //OnTriggerEnter2d

    //Called when this object collides with another.. "other" parameter is what this object collides with
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target"))
        {

            clearTargetHitText();
        }



    }





    private void Update()
    {
        if (gameObject.transform.position.y < -2f)
        {
            gameObject.SetActive(false);
            
        }
        
    }





    private void OnMouseDown ()
    {
        int correctTargetIndex = currentChordLineAndTargetIndexList[count];

        Debug.Log("Pressed left click, casting ray.");
        RaycastHit2D hit = CastRay();

        
        
            if (( hit.collider.CompareTag("Correct Chord") && gameObject.GetComponent<Rigidbody2D>().IsTouching(gm.targets[correctTargetIndex].GetComponent<Collider2D>()))
            )
            {
                txtTargetHit.text = "Uber 1337!";
                correctCount += 1;
                txtChordCount.text = "Chord Count: " + correctCount;

                targetAnimator = gm.targets[correctTargetIndex].gameObject.GetComponent<Animator>();
                targetAnimator.SetTrigger("TargetHit");

                gameObject.SetActive(false);


                count = count + 1;
                if (count < gm.getCorrectChordLineAndTargetIndexList().Count)
                {
                    txtCurrentCorrectChordName.text = "Next Chord: " + gm.getAllChordNames()[gm.getCorrectChordNameIndexList()[count]];
                }
            }
        


    }









    //Called per frame WHILE this object is colliding with an"other" object
    //Probably using this to test if the user is hitting the correct button that corresponds to the correct
    //target over which the correct chord is positioned. Also checking how good the hit is as per the beat
    //where the chord appears in the song.
    private void OnTriggerStay2D(Collider2D other)
    {

        ////currentChordLineManagers = gm.getCurrentChordLineManagers();







        //float correctX;


        //switch (correctTargetIndex)
        //{
        //    case 0:
        //        correctX = -2.25f;
        //        break;
        //    case 1:
        //        correctX = -0.75f;
        //        break;
        //    case 2:
        //        correctX = 0.75f;
        //        break;
        //    case 3:
        //        correctX = 2.25f;
        //        break;
        //    default:
        //        correctX = 0;
        //        break;
        //}


        //float guessedX;
        ////int guessedIndex;

        //if (other.CompareTag("Target"))
        //{

        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        guessedX = -2.25f;
        //    } else if (Input.GetKeyDown(KeyCode.S))
        //    {
        //        guessedX = -0.75f;
        //    }
        //    else if (Input.GetKeyDown(KeyCode.D))
        //    {
        //        guessedX = 0.75f;
        //    }
        //    else if (Input.GetKeyDown(KeyCode.F))
        //    {
        //        guessedX = 2.25f;
        //    } else
        //    {
        //        guessedX = 0;
        //    }



        //    if (correctX == guessedX)
        //    {
        //        correctCount += 1;
        //        txtChordCount.text = "Chord Count: " + correctCount;

        //        txtTargetHit.text = "Meh..";

        //        if (gameObject.transform.position.y < .75f && gameObject.transform.position.y > -.75f)
        //        {
        //            txtTargetHit.text = "Cool";
        //        }
        //        if (gameObject.transform.position.y < .25f && gameObject.transform.position.y > -.25f)
        //        {
        //            txtTargetHit.text = "Woot";
        //        }
        //        if (gameObject.transform.position.y < .1f && gameObject.transform.position.y > -.1f)
        //        {
        //            txtTargetHit.text = "Sauce";
        //        }
        //        if (gameObject.transform.position.y < .05f && gameObject.transform.position.y > -.05f)
        //        {
        //            txtTargetHit.text = "Uber 1337!";
        //        }
        //    } else
        //    {
        //        //wrongCount += 1;
        //        //txtWrongCount.text ="Wrong Count: " + wrongCount;
        //    }


        //}




        int correctTargetIndex = currentChordLineAndTargetIndexList[count];



        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log("Pressed left click, casting ray.");
            RaycastHit2D hit = CastRay();


            
                if (hit.collider.gameObject.name.Equals(targetNames[correctTargetIndex]))
                {
                    txtTargetHit.text = "Uber 1337!";
                    correctCount += 1;
                    txtChordCount.text = "Chord Count: " + correctCount;

                    targetAnimator = gm.targets[correctTargetIndex].gameObject.GetComponent<Animator>();
                    targetAnimator.SetTrigger("TargetHit");

                    gameObject.SetActive(false);


                    count = count + 1;
                    if (count < gm.getCorrectChordLineAndTargetIndexList().Count)
                    {
                        txtCurrentCorrectChordName.text = "Next Chord: " + gm.getAllChordNames()[gm.getCorrectChordNameIndexList()[count]];
                    }

                }
            

       

        }



    }

    
    


    //Method to cast a RaycastHit2D and return what it hits.
    RaycastHit2D CastRay()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        //if (hit.collider != null)
        //{
            //Debug.DrawLine(worldPoint, hit.point);
            Debug.Log("Hit object: " + hit.collider.gameObject.name + "Tag: " + hit.collider.tag);
        //}

        return hit;
    }






    //This happens when the object this script is attached to exits the collision with "other".
    //Using this to clear the TargetHit text after, so it can reappear when another is hit, and isnt just
    //staying on the screen the whole time
    private void OnTriggerExit2D(Collider2D other)
    {

        //Change color back to normal back
        //gm.targets[currentChordLineAndTargetIndexList[count]].gameObject.GetComponent<SpriteRenderer>().color = targetColors[currentChordLineAndTargetIndexList[count]];


        if (other.CompareTag("Target"))
        {
            count = count + 1;
            if (count < gm.getCorrectChordLineAndTargetIndexList().Count)
            {
                txtCurrentCorrectChordName.text = "Next Chord: " + gm.getAllChordNames()[gm.getCorrectChordNameIndexList()[count]];
            }
        }


    }





    public void clearTargetHitText()
    {

        txtTargetHit.text = "";
    }






    //Gets and Sets

    //to set and get the ChordText which is a TextMesh component of the ChordText GameObjeect which is a child of the Chord
    public void setChordText(string s)
    {
        chordText.text = s;
    }
    public TextMesh getChordText()
    {
        return chordText;
    }



}