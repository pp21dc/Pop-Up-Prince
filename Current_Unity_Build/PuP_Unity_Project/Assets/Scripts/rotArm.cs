using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotArm : MonoBehaviour
{
    public GameObject Arm;

    // Update is called once per frame
    void Update()
    {
        Vector3 eArm = Arm.transform.rotation.eulerAngles;
        Quaternion rot = Quaternion.Euler(eArm.x, eArm.y, eArm.z + 180);
        transform.rotation = rot;
    }
}
