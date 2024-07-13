using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CelestialBody : MonoBehaviour
{
    internal PlanetData  _planetData;
    
    [SerializeField]
    private AnimationCurve _scaleCurve;

    // this is km^6
    [SerializeField]
    private float _distanceScale = 0.1f; // this is 10^6 km

    
    // this is km
    [SerializeField]
    
    private float _planetScale = 0.00001f; // scale is in km

    [SerializeField]
    internal Vector3 _actualPosition;


    [SerializeField]
    internal Vector3 _currentVelocity;
    private float _scale = 0.0f;

    public static UnityEvent<float> ScaleChanged = new UnityEvent<float>();
    
    internal Vector3 ScalePosition(Vector3 _actualPosition){
        
        float _maxDiameterScaled = ScaleLocalScale(_planetData.maxDiameter);

        float _scaleConstant = _maxDiameterScaled * _planetData.positionInList * 2f;


        // the x30 is the magic number that makes the planet appear _actualPosition * _distanceScale * 30 away from the sun
        // _actualPostiion is in 10^6 km
        // _distanceScale is 0.1f -> 1.f
        Vector3 _actualPositionScaled = _actualPosition * _distanceScale * 15;

        Vector3 _normalizedPosition = _actualPositionScaled.normalized;

        float lerpedX = Mathf.Lerp(_actualPositionScaled.x, _scaleConstant * _normalizedPosition.x, _scaleCurve.Evaluate(_scale));
        float lerpedY = Mathf.Lerp(_actualPositionScaled.y , _scaleConstant * _normalizedPosition.y, _scaleCurve.Evaluate(_scale));
        float lerpedZ = Mathf.Lerp(_actualPositionScaled.z , _scaleConstant *_normalizedPosition.z, _scaleCurve.Evaluate(_scale));

        return Vector3.Lerp(_actualPositionScaled , new Vector3(lerpedX, lerpedY , lerpedZ),  _scaleCurve.Evaluate(_scale));
        
        // return Mathf.Lerp(distance * _distanceScale , _planetData.maxDiameter * _planetScale * _planetData.positionInList * 2f , _scaleCurve.Evaluate(_scale)) ;
        
        // return Mathf.Lerp(distance * _distanceScale, _planetData.maxDiameter * _planetData.positionInList * 1.5f , _scaleCurve.Evaluate(_scale)) ;
    
    }

    internal float ScaleLocalScale(float diameter){
        return diameter * _planetScale + _scaleCurve.Evaluate(_scale)*_planetData.maxDiameter * _planetScale /2;
        // return diameter * _planetScale + _scaleCurve.Evaluate(_scale)*_planetData.maxDiameter;
    }
    void OnScaleChanged(float scale){
        _scale = scale;
        Planet.ResetTrail.Invoke();
    }

    public bool IsStar(){
        return _planetData.Is_Star;
    }

    internal void OnEnable(){
        ScaleChanged.AddListener(OnScaleChanged);
    }


    internal void OnDisable(){
        ScaleChanged.RemoveListener(OnScaleChanged);
    }

    internal void ChangeActualPosition(Vector3 actualPosition){
        _actualPosition = actualPosition;
    }

    public void CalculateVelocity(List<CelestialBody> celestialBodies, float timeStep){
        foreach(var otherBody in celestialBodies){
            if(otherBody == this){
                continue;
            }

            // Mathf.Sqrt(Vector3.Dot(v, v)) => sqrMagnitude
            float sqrDst = (otherBody._actualPosition - _actualPosition).sqrMagnitude* 100 ;
            Vector3 forceDir = (otherBody._actualPosition - _actualPosition).normalized;
            // mass is in the power of 24
            // position is in the power of 6
            // the constant is 10^-11
            Vector3 force = 6.67430f * _planetData.Mass * otherBody._planetData.Mass * forceDir / sqrDst ;
            Vector3 acceleration = force / _planetData.Mass;
            _currentVelocity += acceleration * timeStep;
        }
    }

    public void UpdatePosition(float timeStep){
        _actualPosition += _currentVelocity * timeStep;
        transform.position = ScalePosition(_actualPosition);
    }

    public void Init(PlanetData planetData)
    {
        _planetData = planetData;

        // calculate scale so the max diameter is 100 ;

        _planetScale = 50 / planetData.maxDiameter;
        

        float scaledLocalScale = ScaleLocalScale(planetData.Diameter);
        transform.localScale = new Vector3(scaledLocalScale, scaledLocalScale, scaledLocalScale);
        transform.position = ScalePosition(_actualPosition);
        name = planetData.Planet;
    }


    public string getName(){
        return _planetData.Planet;
    }


}