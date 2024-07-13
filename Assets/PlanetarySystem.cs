using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class PlanetarySystem
{
    public Dictionary<string , CelestialBody> spawnedCelestialBodies = new();
    private List<Dictionary<string, string>> csvData;

    public List<CelestialBody> _celestialBodies;
    
    private TMP_Dropdown _whereAtDropdown;
    private TMP_Dropdown _lookAtDropdown;

    public void  Enable(){
        foreach (var celestialBody in spawnedCelestialBodies.Values){
            _whereAtDropdown.options.Add(new TMP_Dropdown.OptionData(celestialBody.getName()));
            _lookAtDropdown.options.Add(new TMP_Dropdown.OptionData(celestialBody.getName()));

            celestialBody.gameObject.SetActive(true);
        }
    }

    public void Disable(){
        foreach (var celestialBody in spawnedCelestialBodies.Values){
            celestialBody.gameObject.SetActive(false);
        }    
    }

    public List<CelestialBody> GetCelestialBodies(){
        return spawnedCelestialBodies.Values.ToList(); 
    }

    
    
    Star CreateStar(PlanetData _starData){
        Star _star = GameObject.Instantiate(PlanetDatabase.Instance.starPrefab);
        _star.Init(_starData);
        return _star;
    }

    Planet CreatePlanet(PlanetData _planetData){
        Material _material;
        if(!PlanetDatabase.Instance._materialMap.TryGetValue(_planetData.Planet, out _material)){
            // if i get in here i didn't find a material for the planet
            _material = PlanetDatabase.Instance.generateMaterialFromColors(_planetData.Color.ToLower());

        }
        Planet _planet = GameObject.Instantiate(PlanetDatabase.Instance.planetPrefab);
        _planet.Init(_planetData, _material);
        return _planet;
    }
    
    
    public void Init(string csvName, TMP_Dropdown _whereAtDropdown, TMP_Dropdown _lookAtDropdown){
        this._whereAtDropdown = _whereAtDropdown;
        this._lookAtDropdown = _lookAtDropdown;
        
        CSVReader csvReader = new(); // Create an instance of CSVReader
        csvData = csvReader.LoadCSV(csvName).OrderBy(row => float.Parse(row["Distance from Sun (10^6 km)"])).ToList(); // Call LoadCSV method on the instance

        // find the max diameter
        float maxDiameter = 0;
        foreach (var row in csvData) {
            float diameter = float.Parse(row["Diameter (km)"]);
            if (diameter > maxDiameter) {
                maxDiameter = diameter;
            }
        }

        int i = 0;
          foreach (var row in csvData)
        {
            // create the data
            PlanetData planetData = new(row, maxDiameter, i++);

            if(planetData.Is_Star){
                Star _star = CreateStar(planetData);
                spawnedCelestialBodies.Add(planetData.Planet , _star);
                _star.gameObject.SetActive(false);
            }else {
                Planet _planet = CreatePlanet(planetData);
                spawnedCelestialBodies.Add(planetData.Planet , _planet);
                _planet.gameObject.SetActive(false);
            }
        }

        _celestialBodies = GetCelestialBodies();
    }


    // this is to add a planetary system at run time
    public void Init(List<PlanetData> _planets, TMP_Dropdown _whereAtDropdown, TMP_Dropdown _lookAtDropdown){

        this._whereAtDropdown = _whereAtDropdown;
        this._lookAtDropdown = _lookAtDropdown;

        _planets.ForEach((_planetData)=>{
            if(_planetData.Is_Star){
                Star _star = CreateStar(_planetData);
                spawnedCelestialBodies.Add(_planetData.Planet , _star);
                _star.gameObject.SetActive(false);
            }else {
                Planet _planet = CreatePlanet(_planetData);
                spawnedCelestialBodies.Add(_planetData.Planet , _planet);
                _planet.gameObject.SetActive(false);
            }
        });

        _celestialBodies = GetCelestialBodies();
    }

}