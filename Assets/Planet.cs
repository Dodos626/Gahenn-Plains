using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class Planet : CelestialBody
{

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private TrailRenderer _trailRenderer;

    [SerializeField]
    private MeshRenderer _atmosphereRenderer;

    public static UnityEvent<bool> SetTrail = new UnityEvent<bool>();

    public static UnityEvent ResetTrail = new UnityEvent();

    private Vector3 rotateOrbitalInclination(Vector3 _aphelion , float degrees){

        float radians = - Mathf.Deg2Rad * degrees;

        Unity.Mathematics.float3x3 rotationMatrix = new Unity.Mathematics.float3x3(
            new Unity.Mathematics.float3(Mathf.Cos(radians), -Mathf.Sin(radians), 0),
            new Unity.Mathematics.float3(Mathf.Sin(radians), Mathf.Cos(radians), 0),
            new Unity.Mathematics.float3(0, 0, 1)
        );

        var aphelion = new Unity.Mathematics.float3(_aphelion.x, _aphelion.y, _aphelion.z);

        var res = Unity.Mathematics.math.mul(rotationMatrix, aphelion);

        return new Vector3(res.x, res.y, res.z);
    }


    private Vector3 AverageColor (Texture2D tex ){
        var texColors = tex.GetPixels32();
        var total = texColors.Length;
        var r = 0; var g = 0; var b = 0;
        for (var i = 0; i < total; i++) {
            r += texColors[i].r;
            g += texColors[i].g;
            b += texColors[i].b;
        }
        return new Vector3(r/total, g/total, b/total);
    }


    public void Init(PlanetData planetData, Material material)
    {
        Init(planetData);
        _meshRenderer.material = material;


        // obliquity to orbit is the angle between the plane of the orbit of a planet and the plane of its equator
        _meshRenderer.transform.rotation = Quaternion.Euler(0,_planetData.Obliquity_to_Orbit,0);

        Vector3 _startingPosition = rotateOrbitalInclination(new Vector3(planetData.Aphelion, 0 , 0), planetData.Orbital_Inclination);
        
        ChangeActualPosition(_startingPosition);

        Vector3 positionFromSun =  Vector3.zero - _actualPosition;
        Vector3 perpendicular = rotateOrbitalInclination(Vector3.Cross(positionFromSun, Vector3.up), planetData.Orbital_Inclination);

        _currentVelocity = perpendicular.normalized * _planetData.Orbital_Velocity;


        // coloring

        Texture2D texture = (Texture2D) _meshRenderer.material.GetTexture("_MainTex");

        Vector3 _averageColor = AverageColor(texture).normalized;

        _trailRenderer.startColor = new Color(_averageColor.x ,_averageColor.y, _averageColor.z , 1.0f);
        _trailRenderer.endColor = new Color(_averageColor.x ,_averageColor.y, _averageColor.z , 0.0f);
        _atmosphereRenderer.material.SetVector("_Color",  new Vector4(_averageColor.x ,_averageColor.y, _averageColor.z , 1.0f));

        // the further we are from the sun the more we should extend the trail time
        // for every 100 distance from the sun we add 20 seconds to the trail time
        _trailRenderer.time =  Math.Abs(planetData.Aphelion) / 4;
    }

    // Update is called once per frame
    void Update()
    {
    
        // calculate vector from the sun to this planet
        Vector3 positionFromSun =  _actualPosition - Vector3.zero;
        // pass it to the material
        _atmosphereRenderer.material.SetVector("_LightPosition", positionFromSun.normalized);
       

        float scaledLocalScale = ScaleLocalScale(_planetData.Diameter);
        transform.localScale = new Vector3(scaledLocalScale, scaledLocalScale, scaledLocalScale);

        if(_resetScaleOnNextFrame)
        {
            _trailRenderer.Clear();
            _resetScaleOnNextFrame = false;
            _trailRenderer.widthMultiplier = transform.localScale.x;
        }

    }


    void OnSetTrail(bool trail){
        
        _trailRenderer.enabled = trail;
        _trailRenderer.widthMultiplier = transform.localScale.x;
        _trailRenderer.Clear();
        
    }

    private bool _resetScaleOnNextFrame = false;

    void OnResetTrail(){
        _trailRenderer.Clear();
        _resetScaleOnNextFrame= true;
    }



    new void OnEnable(){
        base.OnEnable();
        SetTrail.AddListener(OnSetTrail);
        ResetTrail.AddListener(OnResetTrail);
    }

    new void OnDisable(){
        base.OnDisable();
        SetTrail.RemoveListener(OnSetTrail);
        ResetTrail.RemoveListener(OnResetTrail);
    }


}
