using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink_Test_3 : MonoBehaviour
{

    public Material Ink_Test_2;

    public float _Width;
    public float _NoiseScaleWidth;
    public float _StrengthWidth;

    public float _Height;
    public float _NoiseScaleHeight;
    public float _StrengthHeight;

    public float _MinHeightAndWidth = 0;
    public float _MaxHeightAndWidth = 0.4f;

    //public float _MinScaleHeightAndWidth = 30;
    //public float _MaxScaleHeightAndWidth = 80;

    public float _HeightAndWidthIncrease = 0.01f;
    //public float _NoiseHeightAndWidthIncrease = 0.1f;

    //float _NoiseScaleWidthCount;
    float _WidthCount;

    //float _NoiseScaleHeightCount;
    float _HeightCount;

    private bool _WidthCountUp = false;
    private bool _HeightCountUp = false;
    //private bool _NoiseScaleWidthCountUp = false;
    //private bool _NoiseScaleHeightCountUp = false;

    // Start is called before the first frame update
    void Start()
    {
        WidthRand();
        HeightRand();

        //NoiseScaleWidthRand();
        //NoiseScaleHeightRand();

        _WidthCount = (_MaxHeightAndWidth - _MinHeightAndWidth) * 0.3f;
        _HeightCount = (_MaxHeightAndWidth - _MinHeightAndWidth) * 0.3f;

        //_NoiseScaleWidthCount = (_MaxScaleHeightAndWidth - _MinScaleHeightAndWidth) * 0.3f;
        //_NoiseScaleHeightCount = (_MaxScaleHeightAndWidth - _MinScaleHeightAndWidth) * 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        //Count Up/Down Start

        //Width Start
        if (_WidthCountUp == true)
        {
            _WidthCount = _WidthCount + _HeightAndWidthIncrease;
            Ink_Test_2.SetFloat("_Width", _WidthCount);
        }

        if (_WidthCountUp == false)
        {
            _WidthCount = _WidthCount - _HeightAndWidthIncrease;
            Ink_Test_2.SetFloat("_Width", _WidthCount);
        }
        //Width End
        //Height Start
        if (_HeightCountUp == true)
        {
            _HeightCount = _HeightCount + _HeightAndWidthIncrease;
            Ink_Test_2.SetFloat("_Height", _HeightCount);
        }

        if (_HeightCountUp == false)
        {
            _HeightCount = _HeightCount - _HeightAndWidthIncrease;
            Ink_Test_2.SetFloat("_Height", _HeightCount);
        }
        //Height End
        //Noise Width Start
        //if (_NoiseScaleWidthCountUp == true)
        //{
        //    _NoiseScaleWidthCount = _NoiseScaleWidthCount + _NoiseHeightAndWidthIncrease;
        //    Ink_Test_2.SetFloat("_Noise_Scale_Width", _NoiseScaleWidthCount);
        //}

        //if (_NoiseScaleWidthCountUp == false)
        //{
        //    _NoiseScaleWidthCount = _NoiseScaleWidthCount - _NoiseHeightAndWidthIncrease;
        //    Ink_Test_2.SetFloat("_Noise_Scale_Width", _NoiseScaleWidthCount);
        //}
        //Noise Width End
        //Noise Height Start
        //if (_NoiseScaleHeightCountUp == true)
        //{
        //    _NoiseScaleHeightCount = _NoiseScaleHeightCount + _NoiseHeightAndWidthIncrease;
        //    Ink_Test_2.SetFloat("_Noise_Scale_Height", _NoiseScaleHeightCount);
        //}
        //
        //if (_NoiseScaleHeightCountUp == false)
        //{
        //    _NoiseScaleHeightCount = _NoiseScaleHeightCount - _NoiseHeightAndWidthIncrease;
        //    Ink_Test_2.SetFloat("_Noise_Scale_Height", _NoiseScaleHeightCount);
        //}
        //Noise Height End

        //Count Up/Down End
        
        //If at Target End

        //Width Start
        if(_WidthCountUp == true && _WidthCount > _Width)
        {
            WidthRand();
        }

        if (_WidthCountUp == false && _WidthCount < _Width)
        {
            WidthRand();
        }
        //Width End
        //Height Start
        if(_HeightCountUp == true && _HeightCount > _Height)
        {
            HeightRand();
        }
        if (_HeightCountUp == false && _HeightCount < _Height)
        {
            HeightRand();
        }
        //Height End
        //Noise Width Start
        //if (_NoiseScaleWidthCountUp == true && _NoiseScaleWidthCount > _NoiseScaleWidth)
        //{
        //   NoiseScaleWidthRand();
        //}
        //if (_NoiseScaleWidthCountUp == false && _NoiseScaleWidthCount < _NoiseScaleWidth)
        //{
        //   NoiseScaleWidthRand();
        //}
        //Noise Width End
        //Noise Height Start
        //if(_NoiseScaleHeightCountUp == true && _NoiseScaleHeightCount > _NoiseScaleHeight)
        //{
        //   NoiseScaleHeightRand();
        //}
        //if (_NoiseScaleHeightCountUp == false && _NoiseScaleHeightCount <_NoiseScaleHeight)
        //{
        //    NoiseScaleHeightRand();
        //}
        //Noise Height End

        //If at Target End
    }

    void WidthRand()
    {
        _Width = Random.Range(_MinHeightAndWidth, _MaxHeightAndWidth);

        if(_Width < _WidthCount)
        {
            _WidthCountUp = false;
        }
        else
        {
            _WidthCountUp = true;
        }
    }

    void HeightRand()
    {
        _Height = Random.Range(_MinHeightAndWidth, _MaxHeightAndWidth);

        if (_Height < _HeightCount)
        {
            _HeightCountUp = false;
        }
        else
        {
            _HeightCountUp = true;
        }
    }

    //void NoiseScaleWidthRand()
    //{
    //    _NoiseScaleWidth = Random.Range(_MinScaleHeightAndWidth, _MaxScaleHeightAndWidth);
    //    if (_Width < _WidthCount)
    //    {
    //        _NoiseScaleWidthCountUp = false;
    //    }
    //    else
    //    {
    //        _NoiseScaleWidthCountUp = true;
    //    }
    //}

    //void NoiseScaleHeightRand()
    //{
    //    _NoiseScaleHeight = Random.Range(_MinScaleHeightAndWidth, _MaxScaleHeightAndWidth);
    //    if (_Width < _WidthCount)
    //    {
    //        _NoiseScaleHeightCountUp = false;
    //    }
    //    else
    //   {
    //       _NoiseScaleHeightCountUp = true;
    //    }
    //}
}
