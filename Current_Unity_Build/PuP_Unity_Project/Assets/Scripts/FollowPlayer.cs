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
    public Quaternion rotation;
    float prevRot;

    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.eulerAngles = -player.eulerAngles/2;
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
    }
}
