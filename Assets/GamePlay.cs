using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{


    // [SerializeField]
    // float timeStep = 0.02f;

    private PlanetarySystem _planetarySystem;

    public static float _currentScale = 0.0f;

    public static void SetScale(float scale){
        _currentScale = scale;
        CelestialBody.ScaleChanged.Invoke(scale);
    }

    
    public static bool _trail = false;
    
    public static void SetTrail(bool trail){
        _trail = trail;
        Planet.SetTrail.Invoke(trail);
    }

    public void StartSystem(PlanetarySystem planetarySystem){

        if(_planetarySystem != null){
            _planetarySystem.Disable();
        }

        _planetarySystem = planetarySystem;

        _planetarySystem.Enable();
        
        SetTrail(_trail);
        SetScale(_currentScale);
        
    }



    // G = Gravitational Constant = 6.67430e-11 N*m^2/kg^2
    // m_1 = Mass Object_1 in kg (e.g. Satellite)
    // m_2 = Mass Object_2 in kg (e.g. Earth)
    // r = distance from Object_1 to Object_2 in meters

    void FixedUpdate(){
        if(_planetarySystem == null){
            return;
        }
        // calculate new velocities
        foreach (var celestialBody in _planetarySystem._celestialBodies){
            celestialBody.CalculateVelocity(_planetarySystem._celestialBodies, Time.fixedDeltaTime);
        }

        // update positions
        foreach (var celestialBody in _planetarySystem._celestialBodies){
            celestialBody.UpdatePosition(Time.fixedDeltaTime);
        }
    }
}
