using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool CameraSettingsBasedOnWorldPosition = true;
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
        if (CameraSettingsBasedOnWorldPosition)
        {
            height = transform.localPosition.y;
            lead = transform.localPosition.x;
            zoom = transform.localPosition.z;
            rotation = transform.rotation.eulerAngles;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (CameraSettingsBasedOnWorldPosition)
        {
            //CameraSettingsBasedOnWorldPosition = false;
            height = transform.localPosition.y;
            lead = transform.localPosition.x;
            zoom = transform.localPosition.z;
            rotation = transform.rotation.eulerAngles;
        }*/
    }
}
