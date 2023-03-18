using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeTower : MonoBehaviour
{

    /**
     * Script to take list of 5 possible tower pieces and randomize which one is shown
     */

    public List<GameObject> towerParts = new List<GameObject>();

    int pieceNum;
    bool hasSpawned = true;

    // this method accepts a list, gets a random piece number in that list
    public void getRandomPiece(List<GameObject> listToRandomize)
    {

        pieceNum = Random.Range(0, towerParts.Count);
     
    }

    // activate the random piece
    public void showPiece()
    {
        towerParts[pieceNum].SetActive(true);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if(hasSpawned == true)
            {
                getRandomPiece(towerParts); // choose a random game object in list
                showPiece(); // show that gameObject
                hasSpawned = false;
            }
        }
    }

}
