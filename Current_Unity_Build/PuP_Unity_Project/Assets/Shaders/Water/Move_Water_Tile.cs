using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Water_Tile : MonoBehaviour
{

    float scrollSpeed;

    float timer;
    GameObject waterSpawn;

    public float deleteAfter = 30;
    // Start is called before the first frame update
    void Start()
    {
        waterSpawn = GameObject.Find("Water Start");

        Scroll_Water scroll_water = waterSpawn.GetComponent<Scroll_Water>();
        scrollSpeed = scroll_water._ScrollSpeed;

        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, 0.001f);

        timer += Time.deltaTime;

        if (timer > deleteAfter)
        {
            Object.Destroy(gameObject);
        }
    }
}
