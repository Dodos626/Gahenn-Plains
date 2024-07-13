using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{

    [SerializeField]
    private Button _toggleTrailButton;

    [SerializeField]
    private Button _toggleUIButton;

    [SerializeField]
    private CanvasGroup _mainUICanvasGroup;

    [SerializeField]
    private Button _addSystemButton;
    
    [SerializeField]
    private StarSystemCreator _starSystemCreator;
    
    [SerializeField]
    private Slider _scaleSlider;
    [SerializeField]
    private Slider _timeSlider;


    private void ToggleTrail(){
        GamePlay.SetTrail(!GamePlay._trail);
        if(!GamePlay._trail){
            _toggleTrailButton.GetComponentInChildren<TMP_Text>().text = "Enable Trail";
        }else{
            _toggleTrailButton.GetComponentInChildren<TMP_Text>().text = "Disable Trail";
        }
    }

    public void SliderChanged(float sliderAmount){
        GamePlay.SetScale(sliderAmount);
    }




    private bool _isUiHidden = false;
    void ToggleUiCanvas(){
        if(_isUiHidden){
            _mainUICanvasGroup.alpha = 1;
            _mainUICanvasGroup.interactable = true;
            _mainUICanvasGroup.blocksRaycasts = true;
            _isUiHidden = false;
        }else{
            _mainUICanvasGroup.alpha = 0;
            _mainUICanvasGroup.interactable = false;
            _mainUICanvasGroup.blocksRaycasts = false;
            _isUiHidden = true;
        }
    }


    void OpenAddSystem(){
        _starSystemCreator.gameObject.SetActive(true);
    }

    void OnEnable(){
        _toggleTrailButton.onClick.AddListener(ToggleTrail);
        _scaleSlider.onValueChanged.AddListener(SliderChanged);
        _timeSlider.onValueChanged.AddListener(OnTimeChanged);
        _toggleUIButton.onClick.AddListener(ToggleUiCanvas);
        _addSystemButton.onClick.AddListener(OpenAddSystem);
    }

    private void OnTimeChanged(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    void OnDisable(){
        _toggleTrailButton.onClick.RemoveListener(ToggleTrail);
        _scaleSlider.onValueChanged.RemoveListener(SliderChanged);
        _timeSlider.onValueChanged.RemoveListener(OnTimeChanged);
        _toggleUIButton.onClick.RemoveListener(ToggleUiCanvas);
        _addSystemButton.onClick.RemoveListener(OpenAddSystem);
    }

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
