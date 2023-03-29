using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSign : MonoBehaviour
{
    float timer;

    public float fallTime = 2f;

    public GameObject Sign;

    public Rigidbody RB1;
    public Rigidbody RB2;

    bool used = false;
    bool ready = false;
    bool once = false;
    // Start is called before the first frame update
    void Start()
    {
        timer = fallTime;
    }

    private void Update()
    {
        if(used == true && once == false)
        {
            RB1.constraints &= ~RigidbodyConstraints.FreezePosition;
            RB2.constraints &= ~RigidbodyConstraints.FreezePosition;

            once = true;
            ready = true;
        }
        timer += Time.deltaTime;
        if(used == true && ready == true && timer > fallTime)
        {
            RB1.constraints = RigidbodyConstraints.FreezePosition;
            RB2.constraints = RigidbodyConstraints.FreezePosition;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && used == false)
        {
            used = true;
            timer = 0f;
        }
}
}
