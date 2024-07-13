using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DataInputLine : MonoBehaviour
{
    public bool IsStar;
    public TMP_InputField Name;
    public TMP_InputField Color;
    public TMP_InputField Mass;
    public TMP_InputField Diameter;
    public TMP_InputField DistanceFromSun;
    public TMP_InputField Aphelion;
    public TMP_InputField OrbitalVelocity;
    public TMP_InputField OrbitalInclination;
    public TMP_InputField ObliquityToOrbit;

    public void Clear()
    {
        Name.text = string.Empty;
        Color.text = string.Empty;
        Mass.text = string.Empty;
        Diameter.text = string.Empty;
        DistanceFromSun.text = string.Empty;
        Aphelion.text = string.Empty;
        OrbitalVelocity.text = string.Empty;
        OrbitalInclination.text = string.Empty;
        ObliquityToOrbit.text = string.Empty;
    }
    

    static float AbetterParseFloat(string s){
        return float.Parse(s,  CultureInfo.InvariantCulture.NumberFormat);
    }

    public PlanetData GetData()
    {
        return new PlanetData(
            Name.text,
            Color.text,
            AbetterParseFloat(Mass.text),
            AbetterParseFloat(Diameter.text),
            AbetterParseFloat(Aphelion.text),
            AbetterParseFloat(OrbitalVelocity.text),
            AbetterParseFloat(OrbitalInclination.text),
            AbetterParseFloat(ObliquityToOrbit.text),
            IsStar
        );
    }
}
