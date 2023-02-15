using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<GameObject> keys = new List<GameObject>();
    public List<GameObject> flowers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetFlowers()
    {
        foreach (GameObject obj in flowers)
        {
            obj.SetActive(true);
        }
    }

    public void resetKeys()
    {
        foreach (GameObject obj in keys)
        {
            obj.SetActive(true);
        }
    }

}
