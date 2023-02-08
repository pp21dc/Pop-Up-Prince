using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink_Change_Test : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    public float height = -1.5f;
    public Material Ink_Test_1;
    public bool goingUp;

    void Start()
    {
        Ink_Test_1.SetFloat("_Cutoff_Height", height);
    }

    void Update()
    {
        if (height == -1.5f)
        {
            goingUp = true;
        }
        if (goingUp == true)
        {
            height = height + 0.001f;
            Ink_Test_1.SetFloat("_Cutoff_Height", height);
        }
        if (height >= .7)
        {
            goingUp = false;
        }
        if (goingUp == false)
        {
            height = height - 0.001f;
            Ink_Test_1.SetFloat("_Cutoff_Height", height);
        }
            
        
    }
}
