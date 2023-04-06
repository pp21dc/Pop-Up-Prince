using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTextUp : MonoBehaviour
{
    public float scrollSpeed = 10000f;
    Vector3 scroll;
    // Start is called before the first frame update
    void Start()
    {
        scroll = new Vector3(0, scrollSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(scroll*Time.deltaTime);
    }
}
