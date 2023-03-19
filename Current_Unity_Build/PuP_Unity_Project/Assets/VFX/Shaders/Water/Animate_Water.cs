using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate_Water : MonoBehaviour
{

    public Material water;

    public float _Max = 100;
    public float _Min = 0;

    public float _Counter;

    public bool countingUp = true;

    public float countIncrease = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        _Counter = _Min;
    }

    // Update is called once per frame
    void Update()
    {
        if (countingUp == true)
        {
            _Counter = _Counter + countIncrease;
            water.SetFloat("_Angle_Offset_Noise", _Counter);
        }

        if (countingUp == false)
        {
            _Counter = _Counter - countIncrease;
            water.SetFloat("_Angle_Offset_Noise", _Counter);
        }

        if (countingUp == true && _Counter > _Max)
        {
            countingUp = false;
        }

        if (countingUp == false && _Counter < _Min)
        {
            countingUp = true;
        }
    }
}
