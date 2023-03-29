using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSign : MonoBehaviour
{
    public Rigidbody RB1;
    public Rigidbody RB2;

    public GameObject sign;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sign.transform.position.y < 8.8)
        {
            RB1.constraints = RigidbodyConstraints.FreezePosition;
            RB2.constraints = RigidbodyConstraints.FreezePosition;
        }
    }
}
