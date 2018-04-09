using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordBackgroundManager : MonoBehaviour {






    public void MoveChordBackground()
    {
        //Fall at 2y per sec -- Time.deltaTime * 1 would be 1 per sec
        transform.position += transform.up * (Time.deltaTime * .5f);
    }



}
