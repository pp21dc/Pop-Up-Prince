using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    public ParticleSystem testSystem;
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            testSystem.Play();
        }
    }
}
