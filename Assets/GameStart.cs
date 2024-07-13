using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

using Cinemachine;
using System.Runtime.Versioning;


public class GameStart : MonoBehaviour
{

    [SerializeField]
    private Material _generalMaterial;

    // this is used when the planets belong to our solar system 
    [SerializeField]
    private List<MaterialReference> _materials ;


    // this is used when the planets are random
    [SerializeField]
    private List<ColorTextureDictionary> _textures ;



    [SerializeField]
    private GamePlay _gamePlay;

    [SerializeField]
    private Planet planetPrefab;

    [SerializeField]
    private Star starPrefab;



    [SerializeField]
    private TMP_Dropdown _whereAtDropdown;

    [SerializeField]
    private TMP_Dropdown _lookAtDropdown;

    [SerializeField]
    private TMP_Dropdown _selectSystemDropdown;

    [SerializeField]
    private Transform _overviewPosition;
    
    [Header("Vcams")]
    [SerializeField]
    private CinemachineVirtualCamera _orbitalCamera;
    [SerializeField]
    private CinemachineVirtualCamera _framingCamera;


    private Dictionary<string , PlanetarySystem> _planetarySystems = new();




    void Awake(){
        PlanetDatabase.Instance.setUpTextures(_textures, _generalMaterial, _materials);
        PlanetDatabase.Instance.SetPrefabs(starPrefab, planetPrefab);
    }



    void AddPlaneterySystem(string systemName, string path){
        PlanetarySystem solarSystem = new();
        solarSystem.Init(path , _whereAtDropdown, _lookAtDropdown);
        _planetarySystems.Add(systemName , solarSystem);

        _selectSystemDropdown.options.Add(new TMP_Dropdown.OptionData(systemName));
    }

    void AddPlaneterySystem(List<PlanetData> planets){
        PlanetarySystem system = new();
        system.Init(planets, _whereAtDropdown, _lookAtDropdown);

        _planetarySystems.Add(planets[0].Planet + " System" , system);
        _selectSystemDropdown.options.Add(new TMP_Dropdown.OptionData(planets[0].Planet + " System"));
    }




    // Start is called before the first frame update
    void Start()
    {
        _selectSystemDropdown.ClearOptions();

        TextAsset[] csvFiles = Resources.LoadAll<TextAsset>("DataSets");
        foreach (TextAsset csvFile in csvFiles)
        {
            AddPlaneterySystem(csvFile.name, $"DataSets/{csvFile.name}");
        }

        _selectSystemDropdown.value = 0;
        _selectSystemDropdown.RefreshShownValue();
        ChangePlanetarySystem(0);

    }

    private string _activeSystem;

    void ChangePlanetarySystem(int key){
        ChangePlanetarySystem(_selectSystemDropdown.options[key].text);
    }

    void ChangePlanetarySystem(string key){
     
        _whereAtDropdown.ClearOptions();
        _lookAtDropdown.ClearOptions();
        _whereAtDropdown.options.Add(new TMP_Dropdown.OptionData("Overview"));


        _activeSystem = key;
        _gamePlay.StartSystem(_planetarySystems[key]);
        
        
        _whereAtDropdown.value = 0;
        _whereAtDropdown.RefreshShownValue();
        _lookAtDropdown.value = 0;
        _lookAtDropdown.RefreshShownValue();
        ChangeView(0);
    }




    
    void ChangeView(int value)
    {

        if(string.IsNullOrEmpty(_activeSystem)){
            return;
        }

        var lookKey = _lookAtDropdown.options[_lookAtDropdown.value].text;
        var whereKey = _whereAtDropdown.options[_whereAtDropdown.value].text;
        
        if (whereKey == "Overview")
        {
            _orbitalCamera.Follow = _overviewPosition;
            _framingCamera.Follow = _overviewPosition;
        }
        else
        {
            var where = _planetarySystems[_activeSystem].spawnedCelestialBodies[whereKey];
            _orbitalCamera.Follow = where.transform;
            _framingCamera.Follow = where.transform;
        }
        
        var lookTarget = _planetarySystems[_activeSystem].spawnedCelestialBodies.TryGetValue(lookKey, out var loock) ? loock.transform : _orbitalCamera.Follow;
        _orbitalCamera.LookAt = lookTarget;
        _framingCamera.LookAt = lookTarget;
        
        if (whereKey == lookKey)
        {
            _framingCamera.Priority = 1;
            _orbitalCamera.Priority = 10;
        }
        else
        {
            _framingCamera.Priority = 10;
            _orbitalCamera.Priority = 1;
        }
    }

    void OnEnable(){
        _whereAtDropdown.onValueChanged.AddListener(ChangeView);
        _lookAtDropdown.onValueChanged.AddListener(ChangeView);
        _selectSystemDropdown.onValueChanged.AddListener(ChangePlanetarySystem);
        StarSystemCreator.OnSavePressed.AddListener(AddPlaneterySystem);
    }

    void OnDisable(){
        _whereAtDropdown.onValueChanged.RemoveListener(ChangeView);
        _lookAtDropdown.onValueChanged.RemoveListener(ChangeView);
        _selectSystemDropdown.onValueChanged.RemoveListener(ChangePlanetarySystem);
        StarSystemCreator.OnSavePressed.RemoveListener(AddPlaneterySystem);
    }

}
