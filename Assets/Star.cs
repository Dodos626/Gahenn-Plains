using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Star : CelestialBody
{




    public void Init(PlanetData planetData, Material material)
    {
        ChangeActualPosition(Vector3.zero);
        Init(planetData);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ScalePosition(new Vector3(0, 0, 0));
        float scaledLocalScale = ScaleLocalScale(_planetData.Diameter);
        transform.localScale = new Vector3(scaledLocalScale, scaledLocalScale, scaledLocalScale);
    }
}
