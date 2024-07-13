using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlanetData
{
    public string Planet;
    public string Color;
    public float Mass ; //(10^24kg);
    public float Diameter;// (km);
    public float Density; //(kg/m^3);
    public float Surface_Gravity;//(m/s^2);
    public float Escape_Velocity ; // (km/s);
    public float Rotation_Period ;// (hours);  the time that the object takes to complete a full rotation around its axis relative to the background stars
    public float Length_of_Day; // (hours);
    public float Distance_from_Sun ;// (10^6 km);
    public float Perihelion ; //(10^6 km); The term "perihelion" refers to the point in the orbit of a planet or other astronomical body, at which it is closest to the sun.
    public float Aphelion;// (10^6 km); the point farthest from the sun in the path of an orbiting celestial body
    public float Orbital_Period; // (days) the amount of time a given astronomical object takes to complete one orbit around another object.
    public float Orbital_Velocity; // (km/s) the orbital speed of an astronomical body or object (e.g. planet, moon, artificial satellite, spacecraft, or star) is the speed at which it orbits around either the barycenter 
    public float Orbital_Inclination; // degrees; Orbital inclination measures the tilt of an object's orbit around a celestial body
    public float Orbital_Eccentricity; // In astrodynamics, the orbital eccentricity of an astronomical object is a dimensionless parameter that determines the amount by which its orbit around another body deviates from a perfect circle. 
    public float Obliquity_to_Orbit; //degrees; Obliquity relates to a planet's plane of orbit. As an orbiting planet spins on its axis, obliquity is the angle between a perpendicular to its orbital plane and its spin axis – the tilt of its axis.
    public float Mean_Temperature; //Celcius;
    public string Surface_Pressure; //bars; string because cases of uknown 
    public float Number_of_Moons;
    public string Ring_System;
    public string Global_Magnetic_Field;

    public bool Is_Star;
    
    // the ones below will be used for scaling purposes
    public float maxDiameter; 
    public int positionInList;




    // ["Planet"];
    // ["Color"];
    // ["Mass (10^24kg)"];
    // ["Diameter (km)"];
    // ["Density (kg/m^3)"];
    // ["Surface Gravity(m/s^2)"];
    // ["Escape Velocity (km/s)"];
    // ["Rotation Period (hours)"];
    // ["Length of Day (hours)"];
    // ["Distance from Sun (10^6 km)"];
    // ["Perihelion (10^6 km)"];
    // ["Aphelion (10^6 km)"];
    // ["Orbital Period (days)"];
    // ["Orbital Velocity (km/s)"];
    // ["Orbital Inclination (degrees)"];
    // ["Orbital Eccentricity"];
    // ["Obliquity to Orbit (degrees)"];
    // ["Mean Temperature (C)"];
    // ["Surface Pressure (bars)"];
    // ["Number of Moons"];
    // ["Ring System?"];
    // ["Global Magnetic Field?"];
    // ["Is Star?]



    public PlanetData(Dictionary<string, string> data, 

        float maxDiameter,
        int positionInList

        )
    {
        Planet = data["Planet"];
        Color = data["Color"];
        Mass = float.Parse(data["Mass (10^24kg)"],  CultureInfo.InvariantCulture.NumberFormat);
        Diameter = float.Parse(data["Diameter (km)"],  CultureInfo.InvariantCulture.NumberFormat);
        Aphelion = float.Parse(data["Aphelion (10^6 km)"],  CultureInfo.InvariantCulture.NumberFormat);
        Orbital_Velocity = float.Parse(data["Orbital Velocity (km/s)"],  CultureInfo.InvariantCulture.NumberFormat);
        Orbital_Inclination = float.Parse(data["Orbital Inclination (degrees)"],  CultureInfo.InvariantCulture.NumberFormat);
        Obliquity_to_Orbit = float.Parse(data["Obliquity to Orbit (degrees)"],  CultureInfo.InvariantCulture.NumberFormat);
        Is_Star = data["Is Star?"] == "Yes" ? true : false;
        this.maxDiameter = maxDiameter;
        this.positionInList = positionInList;
    }


    public PlanetData(string planet, string color, float mass, float diameter,  float aphelion,  float orbital_Velocity, float orbital_Inclination, float obliquity_to_Orbit, bool is_Star)
    {
        Planet = planet;
        Color = color;
        Mass = mass;
        Diameter = diameter;
        Aphelion = aphelion;
        Orbital_Velocity = orbital_Velocity;
        Orbital_Inclination = orbital_Inclination;
        Obliquity_to_Orbit = obliquity_to_Orbit;
        Is_Star = is_Star;
    }

    public void AddExtraData( float maxDiameter, int positionInList){
        this.maxDiameter = maxDiameter;
        this.positionInList = positionInList;
    }

}
