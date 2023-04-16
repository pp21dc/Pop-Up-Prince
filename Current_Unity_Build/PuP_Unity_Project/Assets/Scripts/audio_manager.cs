using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_manager : MonoBehaviour
{
    int x = 0;
    float pitch = 1;
    float targetPitch = 0.16f;
    float pitch_rate = 0.005f;
    public AudioSource AS;
    public int PitchPerSecond = 0;

    // Update is called once per frame
    void Update()
    {
        if (x >= 60* PitchPerSecond && AS.pitch > targetPitch)
        {
            x = 0;
            AS.pitch -= pitch_rate;
        }
        else
        {
            x++;
        }
    }
}
