using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class PlanetDatabase
{

    private static PlanetDatabase instance;

    public static PlanetDatabase Instance {
        get {
            if (instance == null) {
                instance = new PlanetDatabase();
            }
            return instance;
        }
    }

    public List<ColorTextureDictionary> _textures;
    public Dictionary<string, Material> _materialMap = new Dictionary<string, Material>();

    private List<Texture2D> allTextures = new();

    public Material _generalMaterial;

    public List<Texture2D> getAllTexturesForColor(string color){
        foreach (var colorTexture in _textures)
        {
            if(colorTexture.ColorName == color){
                return colorTexture.texture;
            }
        }
        return null;
    }

    public Material generateMaterialFromColors(string color) {
        // split string at space
        string[] colors = color.Split(' ');
        // get all colors for the planet

        List<Texture2D> totalPossibleTextures = new();

        foreach (var colorName in colors){
            List<Texture2D> textures = getAllTexturesForColor(colorName);
            if(textures != null){
                totalPossibleTextures.AddRange(textures);
            }
        }

        Material _mat = new(_generalMaterial.shader);
        
        // if the total possible textures is not empty pick one at random
        if(totalPossibleTextures.Count > 0){
            _mat.SetTexture("_MainTex", totalPossibleTextures[UnityEngine.Random.Range(0, totalPossibleTextures.Count)]);
            //  = totalPossibleTextures[UnityEngine.Random.Range(0, totalPossibleTextures.Count)];
            return _mat;
        }


        // else pick one from the totality of colors at random
        _mat.SetTexture("_MainTex", allTextures[UnityEngine.Random.Range(0, allTextures.Count)]);
        return _mat;
        
    }

    public Star starPrefab;
    public Planet planetPrefab;


    public void setUpTextures(List<ColorTextureDictionary> _textures ,Material _generalMaterial , List<MaterialReference> _materials){

        this._textures = _textures;
        this._generalMaterial = _generalMaterial;
        foreach (var colorTexture in _textures)
        {
            allTextures.AddRange(colorTexture.texture);
        }
        // create the material map
        foreach (var materialReference in _materials)
        {
           _materialMap.Add(materialReference.PlanetName, materialReference.material);
        }

    }

    public void SetPrefabs(Star starPrefab, Planet planetPrefab){
        this.starPrefab = starPrefab;
        this.planetPrefab = planetPrefab;
    }



}