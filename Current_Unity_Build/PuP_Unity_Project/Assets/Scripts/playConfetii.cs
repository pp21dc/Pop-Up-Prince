using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playConfetii : MonoBehaviour
{

    public ParticleSystem red;
    public ParticleSystem blue;
    public ParticleSystem green;
    public ParticleSystem yellow;
    public ParticleSystem orange;
    public ParticleSystem purple;
    public void playAllConfetti()
    {
        red.Play();
        blue.Play();
        green.Play();
        yellow.Play();
        orange.Play();
        purple.Play();
    }
}
