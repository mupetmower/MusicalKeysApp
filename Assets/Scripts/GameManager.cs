using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour {

    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    public Color[] tColors = new Color[4];
    
    
    
    //List to hold all target Game Objects -- may change to List
    public List<GameObject> targets = new List<GameObject>();



    //variable to hold each chord GameObject's ChordManager script component
    public List<ChordManager> chords;

    //Hold the Chord Prefab to use within here
    public GameObject chord;

    //ChordBackground Prefab
    public GameObject chordBackground;

    //List of all ChordBackground Script Components
    public List<ChordBackgroundManager> chordBackgroundList;



    //To hold all the Text Compnenets of the Text GameObject under Canvas
    public Text txtSeconds;
    public Text txtTargetHit;
    public Text txtChordNameTest;
    public Text txtChordCount;
    public Text txtTimeCount;
    public Text txtCurrentCorrectChordName;


    //board holder to place new GaemObjects and clean up
    private Transform boardHolder;


    //Holding the current chord line's names
    public List<string> currentChordLineNames = new List<string>();

    //Holding the current chord line's GameObjects too be initialized
    public List<GameObject> currentChordLineObjs = new List<GameObject>();

    //Holding the x(0-3) to use as index for assigning the x coordinate for chord line chords
    public List<int> currentChordLineXValues = new List<int>();

    //Holds list of all possible chord names
    List<string> allChordNames = new List<string>(14){ "A", "B", "C", "D", "E", "F", "G", "a", "b", "c", "d", "e", "f", "g" };

    //This will hold the correct chord and target index for the Lists -- will be Rand(0-3) per chord line
    int correctChordLineAndTargetIndex;
    //to hold a List of all the correct chordLineAndTargetIndex numbers
    public List<int> correctChordLineAndTargetIndexList = new List<int>();

    //This will hold the index for the correct chord name to use with allChordNames to get the right name per chord line
    //int correctChordLineNameIndex = -1;

    //Rand(0-allChordNames.Count) to randomly choose the other 3 chords in the chord line
    int randChord;
    //Rand(0-totalTargetNum-1) to randomly select a num to assign each x for each chord in current line
    int randNum;

    //Total count of all targets may change per difficulty
    public int totalTargetNum = 4;


    FileInfo fileToRead = new FileInfo("Resources/FirstTestSongChords.txt");
    public FileInfo secondFile = new FileInfo("Assets/Resources/SecondTestSongChords.txt");



    //StreamReader to read the text file corresponding to the current song
    StreamReader reader;

    List<int> correctChordNameIndexList = new List<int>();


    //Array to hold each y coord per ChordName char in file
    int[] allYsFromFile = new int[50];



    public TextAsset file = new TextAsset();



    //Awake is always called before any Start functions
    void Awake()
    {
        
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        else if (instance != this)  //If instance already exists and it's not this,
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }


        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);



        boardHolder = new GameObject("Board").transform;


        //Setting Text components to their variables
        txtSeconds = GameObject.Find("Seconds").GetComponent<Text>();
        txtTargetHit = GameObject.Find("TargetHit").GetComponent<Text>();
        txtChordCount = GameObject.Find("ChordCount").GetComponent<Text>();
        txtTimeCount = GameObject.Find("TimeCount").GetComponent<Text>();
        txtChordNameTest = GameObject.Find("ChordNameTest").GetComponent<Text>();
        txtCurrentCorrectChordName = GameObject.Find("CurrentCorrectChordName").GetComponent<Text>();

        

        //Adding all target GameObjects to target Array -- may change to list
        targets.Add(GameObject.Find("Target"));
        targets.Add(GameObject.Find("Target (1)"));
        targets.Add(GameObject.Find("Target (2)"));
        targets.Add(GameObject.Find("Target (3)"));





        //Instantiate first ChordBackgroundTile
        GameObject toInstantiate = chordBackground;
        GameObject firstChordBackgroundTile;
        firstChordBackgroundTile = Instantiate(toInstantiate, new Vector3(0f, 10f, 0f), Quaternion.identity);
        firstChordBackgroundTile.transform.SetParent(boardHolder);
        chordBackgroundList.Add(firstChordBackgroundTile.GetComponent<ChordBackgroundManager>());





        //This was for using Streamreader and FileInfo Objects
        ////tell StreamReader to open secdonFile's text..
        //reader = secondFile.OpenText();
        ////and read it to the end, putting it all as one string in fullFileText var
        //string fullFileText = reader.ReadToEnd();



        //Using TextAsset instead of FileInfo or File, so it will still be able to be used in Mobile Builds
        string fullFileText = file.text;

        //split the file at each "," and place into a string[]
        string[] splitFullFile = Regex.Split(fullFileText, ",");

        //for each element in the string array...
        foreach (string e in splitFullFile)
        {
            //string chordNamePattern = @"[a-zA-Z]";
            //string beatNumberPattern = @"(\d+)(.\d+)?";


            // create a sub string of the first character
            string cName = e.Substring(0, 1);
            //and another of the rest of the characters after the first
            string bNum = e.Substring(1);

            //parse the bNum to a float and convert it to the y-coordinate to put the next chord at
            // -1 since the targets are at 0y, and then +8 to make them start at 8y
            float nextY = float.Parse(bNum) - 1f + 8f;

            //add the chord name indec to the list
            correctChordNameIndexList.Add(allChordNames.IndexOf(cName));

            //Initialize the new chord with it's new parameters, and the rest of it's line
            InitNewChords(nextY, cName);



        } 

        //close and dispose of the StreamReader.
        //reader.Close();
        //reader.Dispose();



        //Show the player the first chord
        txtCurrentCorrectChordName.text = "Next Chord: " + allChordNames[correctChordNameIndexList[0]];








        //reader = fileToRead.OpenText();

        //int fileCharCount = 0;
        //int testY = 8;
        //int nextChordInFileAsACII = -2;


        //while (nextChordInFileAsACII != -1)
        //{
        //    //reading each character and returning an int, which is the ACII that corresponds to the character which is read
        //    //and assigning it to a variable to use will return -1 if there is no character
        //    //-- this will possibly be the end, or maybe might use reader.EndOfStream() to return bool
        //    nextChordInFileAsACII = reader.Read();
        //    if (nextChordInFileAsACII != -1)
        //    {
        //        string nextChord;
        //        nextChord = System.Convert.ToChar(nextChordInFileAsACII).ToString();

        //        if (!nextChord.Equals(","))
        //        {
        //            allYsFromFile[fileCharCount] = testY;

        //            switch (nextChord)
        //            {
        //                case "A":
        //                    //correctChordLineNameIndex = 0;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "B":
        //                    //correctChordLineNameIndex = 1;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "C":
        //                    //correctChordLineNameIndex = 2;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "D":
        //                    //correctChordLineNameIndex = 3;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "E":
        //                    //correctChordLineNameIndex = 4;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "F":
        //                    //correctChordLineNameIndex = 5;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "G":
        //                    //correctChordLineNameIndex = 6;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "a":
        //                    //correctChordLineNameIndex = 7;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "b":
        //                    //correctChordLineNameIndex = 8;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "c":
        //                    //correctChordLineNameIndex = 9;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "d":
        //                    //correctChordLineNameIndex = 10;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "e":
        //                    //correctChordLineNameIndex = 11;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "f":
        //                    //correctChordLineNameIndex = 12;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                case "g":
        //                    //correctChordLineNameIndex = 13;
        //                    InitNewChords(allYsFromFile[fileCharCount], nextChord);
        //                    break;
        //                //case ",":
        //                //    fileCharCount -= 1;
        //                //    break;
        //                default:
        //                    break;
        //            }
        //            correctChordNameIndexList.Add(allChordNames.IndexOf(nextChord));
        //        }
        //        else
        //        {
        //            testY -= 4;
        //            fileCharCount -= 1;
        //        }

        //        testY += 4;
        //        fileCharCount += 1;


        //    }
        //    else
        //    {
        //        reader.Close();
        //        reader.Dispose();
        //    }

        //}


    }


    



    // Use this for initialization
    void Start () {

        //Adding all Chord GameObjects in currently game to chordHolder,
        //chordHolder = GameObject.FindGameObjectsWithTag("Chord");

        ////Then looping through it and adding each ChordManager Script Component to chords List
        //for (int i = 0; i < chordHolder.Length; i++)
        //{
        //    chords.Add(chordHolder[i].GetComponent<ChordManager>());
        //}

        

        //Test to change the text to the right chord name from this script
        //chords[0].setChordText("A");
        //chords[1].setChordText("B");




    }
	




	// Update is called once per frame
	void Update () {
        //Counting the seconds since game began
        txtSeconds.text = Time.realtimeSinceStartup.ToString();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Application.Quit();
        }



        
        

    }



    //Called independant of frame rate. used for physics
    void FixedUpdate()
    {
        //Move each chord in list
        foreach (ChordManager cm in chords)
        {
            cm.MakeChordFall();
        }


        //Move each chordBackground in list
        foreach (ChordBackgroundManager cbgm in chordBackgroundList)
        {
            //cbgm.MoveChordBackground();

        }



    }


    



    //InitNewChords


    //Initialize the new Chords.. possibly passing in int y as the y coordinate to put each new chord line at
    public void InitNewChords(float y, string currentChordName)
    {

        //clear out arrays needed to complete whiles and hold new random chords
        currentChordLineNames.Clear();
        currentChordLineXValues.Clear();


        //Choose and add the correct, and also 3 random chord names to use for chordLine


        //Random Chord Name Generator

        //Variable to use while checking if the newly chosen rand chord is the same as any in the current
        //new chord line
        bool sameChordNameCheck;

        //Firstly, add the correct chord  coming up for each new chord line
        currentChordLineNames.Add(currentChordName);

        //Next, loop until the chord line reaches 4 elements
        while (currentChordLineNames.Count != 4)
        {
            //change same name back to true at start of each loop
            sameChordNameCheck = true;

            //while they are the same name,
            while (sameChordNameCheck == true)
            {
                //create a new rand chord
                randChord = Random.Range(0, allChordNames.Count);
                //and assign the name to a temp variable
                string temp = allChordNames[randChord];

                //for each element in currentChordLineNames
                foreach (string item in currentChordLineNames)
                {
                    //make sameNameCheck false to set it to break loop, unless they are the same in the if check,
                    sameChordNameCheck = false;
                    //if they ARE the same,
                    if (item.Equals(temp))
                    {
                        //Change sameName back to true so it willl break the loop since you dont
                        //need to check any of the others elements since one is already the same,
                        sameChordNameCheck = true;
                        //and force a break
                        break;
                    }

                }
                //Now, after it has checked all of the elements in currentChordLineNames, and found that the temp randChord
                //is either the same or not,
                //Check to see if it is NOT the sameName,
                if (sameChordNameCheck == false)
                {
                    //And if its NOT, then add the temp randChord to the List
                    currentChordLineNames.Add(temp);
                }
                //Then continue the while loop that is cheakcing sameName. If, inside the foreach, the if found sameName was
                //true, then it will loop the while, having not added anything to the List, and then create a new randChord
                //to store in temp and check all over again. if it found that the sameName was false, then it would have added to the list,
                //and exited the while since sameName was not true anymore.
            }
            //At this point, the first while will loop and start all over if
            //there still arent 4 ChordNames in the List. If there arent, it changes sameName back to true, to restart the proccess,
            //entering the sameName while again.
        }

        //Listing each chord name in List after they are randomly chosen
        foreach (string item in currentChordLineNames)
        {
            txtChordNameTest.text += item + ", ";
        }







        //Initialize new currentChordLine and add to game


        //Choosing the 4 random x values for each chord in line

        //Random x value Generator

        //Create the numbers for new chord line x coordinates
        //Useing exact same logic as the Random Chord Name Generator
        bool sameNumCheck;

        int randCorrectIndexNum = Random.Range(0, totalTargetNum);
        currentChordLineXValues.Add(randCorrectIndexNum);

        //set correct ChordLine and Target index to new rand num
        correctChordLineAndTargetIndex = randCorrectIndexNum;

        correctChordLineAndTargetIndexList.Add(correctChordLineAndTargetIndex);

        while (currentChordLineXValues.Count != 4)
        {
            
            sameNumCheck = true;
            while (sameNumCheck == true)
            {

                randNum = Random.Range(0, totalTargetNum);
                int tempNum = randNum;
                foreach (int item in currentChordLineXValues)
                {
                    sameNumCheck = false;

                    if (item == tempNum)
                    {
                        sameNumCheck = true;
                        break;
                    }

                }
                
                if (sameNumCheck == false)
                {
                    currentChordLineXValues.Add(tempNum);
                }

            }

        }


        //add new nums to ChordNameTest
        foreach (int item in currentChordLineXValues)
        {
            txtChordNameTest.text += item + " ";
        }



        //Create a Chord GameObject for each of the 4 chords in the new chord line. then instantiate them in their proper random position






        for (int i = 0; i < currentChordLineXValues.Count; i++)
        {
            float x;
            GameObject toInstantiate = chord;
            GameObject createdChord;
            ChordManager createdChordManager;
            SpriteRenderer sr;

            switch (currentChordLineXValues[i])
            {
                case 0:
                    x = -2.25f;
                    createdChord = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                    createdChordManager = createdChord.GetComponent<ChordManager>();
                    createdChordManager.setChordText(currentChordLineNames[i]);
                    
                    
                    sr = createdChord.GetComponent<SpriteRenderer>();
                    sr.color = tColors[currentChordLineXValues[i]];

                    if (i == 0)
                    {
                        createdChord.tag = "Correct Chord";
                        createdChord.GetComponentInChildren<TextMesh>().tag = "Correct Chord";
                        createdChord.GetComponent<CircleCollider2D>().enabled = true;
                    }

                    createdChord.transform.SetParent(boardHolder);

                    chords.Add(createdChordManager);
                    break;
                case 1:
                    x = -0.75f;
                    createdChord = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                    createdChordManager = createdChord.GetComponent<ChordManager>();
                    createdChordManager.setChordText(currentChordLineNames[i]);
                    sr = createdChord.GetComponent<SpriteRenderer>();
                    sr.color = tColors[currentChordLineXValues[i]];

                    if (i == 0)
                    {
                        createdChord.tag = "Correct Chord";
                        createdChord.GetComponent<CircleCollider2D>().enabled = true;
                    }

                    createdChord.transform.SetParent(boardHolder);

                    chords.Add(createdChordManager);
                    break;
                case 2:
                    x = 0.75f;
                    createdChord = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                    createdChordManager = createdChord.GetComponent<ChordManager>();
                    createdChordManager.setChordText(currentChordLineNames[i]);
                    sr = createdChord.GetComponent<SpriteRenderer>();
                    sr.color = tColors[currentChordLineXValues[i]];

                    if (i == 0)
                    {
                        createdChord.tag = "Correct Chord";
                        createdChord.GetComponent<CircleCollider2D>().enabled = true;
                    }

                    createdChord.transform.SetParent(boardHolder);

                    chords.Add(createdChordManager);
                    break;
                case 3:
                    x = 2.25f;
                    createdChord = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                    createdChordManager = createdChord.GetComponent<ChordManager>();
                    createdChordManager.setChordText(currentChordLineNames[i]);
                    sr = createdChord.GetComponent<SpriteRenderer>();
                    sr.color = tColors[currentChordLineXValues[i]];

                    if (i == 0)
                    {
                        createdChord.tag = "Correct Chord";
                        createdChord.GetComponent<CircleCollider2D>().enabled = true;
                    }

                    createdChord.transform.SetParent(boardHolder);
                    

                    chords.Add(createdChordManager);
                    break;
                    
            }

            
            
            

        }

        



    }

    





    //Gets and Sets


    //Possibly to use in ChordManager to get target Array/List to test to see if index of 
    //target chosen by player is the correct index of the correct chord
    //public List<GameObject> getTargets()
    //{
    //    return targets;
    //}

    public List<int> getCorrectChordNameIndexList()
    {
        return correctChordNameIndexList;
    }

    public List<string> getAllChordNames()
    {
        return allChordNames;
    }
    
    public List<int> getCorrectChordLineAndTargetIndexList()
    {
        return correctChordLineAndTargetIndexList;
    }

    public float getCorrectChordLineAndTargetXPos()
    {
        int n = correctChordLineAndTargetIndex;
        float x = 0;

        switch (n)
        {
            case 0:
                x = -2.25f;
                return x;
                
            case 1:
                x = -0.75f;
                return x;
                
            case 2:
                x = 0.75f;
                return x;
                
            case 3:
                x = 2.25f;
                return x;
            default:
                return 0f;
        }
        
    }


    public Transform getBoard()
    {
        return boardHolder;
    }

    
}