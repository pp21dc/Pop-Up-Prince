using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{

    public float friction = 32f;

    [HideInInspector]
    public bool top;
    [HideInInspector]
    public bool left;
    [HideInInspector]
    public bool right;
    [HideInInspector]
    public bool roof;
    [HideInInspector]
    public Transform trans;
    [HideInInspector]
    public float slope;
    [HideInInspector]
    public Vector3 leftSide;
    [HideInInspector]
    public Vector3 rightSide;
    [HideInInspector]
    public float verticalWidthAP;//Vertical width at anypoint in the objects slope

    private void Start()
    {
        RectTransform rt;
        trans = gameObject.transform;
        Quaternion rot = gameObject.transform.rotation;
        float tanResult = 0;
        leftSide = transform.position;
        rightSide = transform.position;
        
        transform.SetPositionAndRotation(transform.position, new Quaternion());

        MeshFilter mF = GetComponent<MeshFilter>();
        float widthY = mF.mesh.bounds.extents.y * transform.localScale.y;
        float widthX = mF.mesh.bounds.extents.x * transform.localScale.x;
        GameObject tempLeft = transform.GetChild(0).gameObject;
        GameObject tempRight = transform.GetChild(1).gameObject;
        tempLeft.transform.position -= new Vector3(widthX,-widthY);
        tempRight.transform.position += new Vector3(widthX,widthY);


        transform.SetPositionAndRotation(transform.position, rot);

        leftSide = tempLeft.transform.position;
        rightSide = tempRight.transform.position;

        
        

        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 90)
        {
            float rad = transform.eulerAngles.z * Mathf.PI / 180;
            tanResult = Mathf.Tan(rad);
            //Debug.Log(tanResult);
            float rightSideY = rightSide.x * Mathf.Tan(rad);
            rightSide = new Vector3(rightSide.x, rightSideY, 0);

            float RadAngle = (90 - transform.eulerAngles.z) * Mathf.PI / 180;

            verticalWidthAP = Mathf.Abs(widthY / (Mathf.Sin(RadAngle)));

        }
        else if (transform.eulerAngles.z < 0 || transform.eulerAngles.z > 90)
        {
            float rad = (360f - Mathf.Abs(transform.eulerAngles.z)) * Mathf.PI / 180;
            tanResult = Mathf.Tan(rad) / -1;
            float leftSideY = rightSide.x * Mathf.Tan(rad);
            leftSide = new Vector3(leftSide.x, leftSideY, 0);

            float RadAngle = Mathf.Abs((270f - transform.eulerAngles.z)) * Mathf.PI / 180;
            
            verticalWidthAP = widthY / Mathf.Sin(RadAngle);
        }
        slope = tanResult;
    }
}
