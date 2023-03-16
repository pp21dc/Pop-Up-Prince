using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Water : MonoBehaviour
{

    public float _ScrollSpeed;

    public GameObject WaterTile;

    public GameObject waterStart;
    Vector3 waterStartLocation;
    public float spawnTimeGap;
    public float Timer;
    public bool enabled;
    Vector3 objectScale;
    // Start is called before the first frame update
    void Start()
    {
        objectScale = WaterTile.GetComponent<Renderer>().bounds.size;

        _ScrollSpeed = 0.01f;
        Timer = 0;
        waterStartLocation = GameObject.Find("Water Start").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Timer = Timer + Time.deltaTime;

        if (Timer > 4 && enabled == true)
        {
            GameObject water = Instantiate(WaterTile, transform.position, transform.rotation);
            Timer = 0;
        }
    }
}
