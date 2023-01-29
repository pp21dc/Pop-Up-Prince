using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink_Test_2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float height2 = -1.5f;
    public Material Ink_Test_New;
    public bool goingUp2;

    void Start()
    {
        Ink_Test_New.SetFloat("_Cutoff_Height", height2);
    }

    void Update()
    {
        if (height2 == -1.5f)
        {
            goingUp2 = true;
        }
        if (goingUp2 == true)
        {
            height2 = height2 + 0.001f;
            Ink_Test_New.SetFloat("_Cutoff_Height", height2);
        }
        if (height2 >= .7)
        {
            goingUp2 = false;
        }
        if (goingUp2 == false)
        {
            height2 = height2 - 0.001f;
            Ink_Test_New.SetFloat("_Cutoff_Height", height2);
        }


    }
}
