using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink_Test_3 : MonoBehaviour
{

    public Material Ink_Test_2;

/*    public float _Width;
    public float _NoiseScaleWidth;
    public float _StrengthWidth;*/

/*    public float _Height;
    public float _NoiseScaleHeight;
    public float _StrengthHeight;*/
/*
    public float _MinHeightAndWidth = 0;
    public float _MaxHeightAndWidth = 0.4f;*/

    public float _Voronoi_Increase_Rate = 0.01f;
    public float _Voronoi_Height = 0;
    public float _Voronoi_Width = 0;
    public float _Voronoi_Blotch = 0;
    float _Voronoi_Increase;
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
/*        WidthRand();
        HeightRand();*/

        //NoiseScaleWidthRand();
        //NoiseScaleHeightRand();

        /*_WidthCount = (_MaxHeightAndWidth - _MinHeightAndWidth) * 0.3f;
        _HeightCount = (_MaxHeightAndWidth - _MinHeightAndWidth) * 0.3f;*/

        //_NoiseScaleWidthCount = (_MaxScaleHeightAndWidth - _MinScaleHeightAndWidth) * 0.3f;
        //_NoiseScaleHeightCount = (_MaxScaleHeightAndWidth - _MinScaleHeightAndWidth) * 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        _Voronoi_Increase = _Voronoi_Increase_Rate * Time.deltaTime;
        _Voronoi_Width += _Voronoi_Increase;
        _Voronoi_Height += _Voronoi_Increase;
        _Voronoi_Blotch += _Voronoi_Increase;
        Ink_Test_2.SetFloat("_Width_Voronoi_Offset", _Voronoi_Width);
        Ink_Test_2.SetFloat("_Height_Voronoi_Offset", _Voronoi_Height);
        Ink_Test_2.SetFloat("_Transparency_Voronoi_Offset", _Voronoi_Blotch);
        /* //Count Up/Down Start
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
         }*/
    }

/*    void WidthRand()
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
    }*/
}
