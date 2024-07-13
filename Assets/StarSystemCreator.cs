using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StarSystemCreator : MonoBehaviour
{
    public static UnityEvent<List<PlanetData>> OnSavePressed = new();
    
    [Header("Refs")]
    [SerializeField]
    private RectTransform _container;
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private ScrollRect _scrollRect;
    
    [SerializeField]
    private DataInputLine _startDataLine;

    [SerializeField]
    private Button _addButton;
    [SerializeField]
    private Button _saveButton;
        [SerializeField]
    private Button _cancelButton;
    
    [Header("Prefabs")]
    [SerializeField]
    private DataInputLine _dataLinePrefab;

    private readonly List<DataInputLine> _planets = new();


    private void CancelCreation(){
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;

        _startDataLine.Clear();
        _planets.Clear();
        
        _scrollRect.gameObject.SetActive(true);
        
        _addButton.onClick.AddListener(CreatPlanetLine);
        _saveButton.onClick.AddListener(SaveSystem);
        _cancelButton.onClick.AddListener(CancelCreation);
    }

    private void OnDisable()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;

        foreach (var planet in _planets)
        {
            Destroy(planet.gameObject);
        }

        _startDataLine.Clear();
        _planets.Clear();
        
        _scrollRect.gameObject.SetActive(false);
        
        _addButton.onClick.RemoveListener(CreatPlanetLine);
        _saveButton.onClick.RemoveListener(SaveSystem);
        _cancelButton.onClick.RemoveListener(CancelCreation);

    }

    private void SaveSystem()
    {
        var data = new List<PlanetData>
        {
            //Add star first
            _startDataLine.GetData()
        };

        //Then add all planets
        foreach (var planet in _planets)
        { 
            data.Add(planet.GetData());
        }
        // order by aphelion
        List<PlanetData> orderedPlanets =  data.OrderBy(_planet => _planet.Aphelion).ToList();
        
        // update with max diameter and position in list
        float maxDiameter = 0;
        foreach (var _planet in orderedPlanets) {
            float diameter = _planet.Diameter;
            if (diameter > maxDiameter) {
                maxDiameter = diameter;
            }
        }

        for (int i = 0; i < orderedPlanets.Count; i++)
        {
            orderedPlanets[i].AddExtraData(maxDiameter, i);
        }

        

        //Can Validate data here
        
        OnSavePressed.Invoke(orderedPlanets);
        gameObject.SetActive(false);
    }

    private void CreatPlanetLine()
    {
        var newLine = Instantiate(_dataLinePrefab, _container);
        _planets.Add(newLine);
    }
}
