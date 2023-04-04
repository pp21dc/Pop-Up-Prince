using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink_Animate : MonoBehaviour
{

    public Material Material;

    /*    public float _Width;
        public float _NoiseScaleWidth;
        public float _StrengthWidth;*/

    /*    public float _Height;
        public float _NoiseScaleHeight;
        public float _StrengthHeight;*/
    /*
        public float _MinHeightAndWidth = 0;
        public float _MaxHeightAndWidth = 0.4f;*/

    [Header("Values for debugging/testing while running")]
    public float _Voronoi_Increase_Rate = 0.01f;
    public float _Voronoi_Height = 0;
    public float _Voronoi_Width = 0;
    public float _Voronoi_Blotch = 0;


    float _Voronoi_Increase;

    public float _HeightAndWidthIncrease = 0.01f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _Voronoi_Increase = _Voronoi_Increase_Rate * Time.deltaTime;
        _Voronoi_Width += _Voronoi_Increase;
        _Voronoi_Height += _Voronoi_Increase;
        _Voronoi_Blotch += _Voronoi_Increase;
        Material.SetFloat("_Width_Voronoi_Offset", _Voronoi_Width);
        Material.SetFloat("_Height_Voronoi_Offset", _Voronoi_Height);
        Material.SetFloat("_Transparency_Voronoi_Offset", _Voronoi_Blotch);

    }

}