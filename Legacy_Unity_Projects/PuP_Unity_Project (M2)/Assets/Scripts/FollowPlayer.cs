using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public Transform player;
    public float height;
    public float lead;
    public float zoom;
    public float followSpeed;
    public Vector3 rotation;
    float prevRot;

    // Start is called before the first frame update
    void Start()
    {
        //rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = new Quaternion(0, 0, 500, 0);
    }
}
