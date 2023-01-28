using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink_Change_Test : MonoBehaviour
{
    // Start is called before the first frame update
    float height = -1.5f;
    void Start()
    {
        Shader.SetGlobalFloat("_Cutoff_Height", height);
    }

    // Update is called once per frame
    void Update()
    {
        height = height + 0.01f;
        GetComponent<Renderer>().sharedMaterial.SetFloat("_Cutoff_Height", height);
        print(height);

    }
}
