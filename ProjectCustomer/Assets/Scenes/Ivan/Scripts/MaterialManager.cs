using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public enum DestinationType {
        DEFAULT = 0,
        CHURCH = 1,
        MARKET = 2,
        CAFE = 3,
        PARK = 4,
        NURSERY = 5,
        OLD_HOME = 6
    }
    float recognitionDistance = 5;
    int destinationID;
    float destinationDistance = 50;
    public List<Material> materials = new List<Material>();
    public static MaterialManager instance;

    DestinationType GetMaterialByID(int ID)
    {
        switch (ID)
        {
            case 0: return DestinationType.DEFAULT;
            case 1: return DestinationType.DEFAULT;
            case 2: return DestinationType.CHURCH;
            case 3: return DestinationType.CHURCH;
            case 4: return DestinationType.NURSERY;
            case 5: return DestinationType.CAFE;
            case 6: return DestinationType.MARKET;
            case 7: return DestinationType.PARK;
            case 8: return DestinationType.OLD_HOME;
            case 9: return DestinationType.DEFAULT;
        }
        return DestinationType.DEFAULT;
    }
    List<Material> GetMaterialsByType(DestinationType type)
    {
        List<Material> list = new List<Material>();
        for (int i = 0; i < materials.Count; i++)
            if (GetMaterialByID(i) == type)
                list.Add(materials[i]);
        return list;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        RemoveAllDestinations();
        if (GlobalData.instance != null)
        {
            GlobalData.instance.OnStoryChange.AddListener(StoryUpdated);
        }
    }
    public void SetDestination(DestinationType materialID)
    {
        List<Material> selection = GetMaterialsByType(materialID);
        foreach (Material mat in selection)
        {
            SetDestination(mat);
        }
    }
    public void RemoveDestination(DestinationType materialID)
    {
        List<Material> selection = GetMaterialsByType(materialID);
        foreach (Material mat in selection)
        {
            RemoveDestination(mat);
        }
    }
    public void RemoveAllDestinations()
    {
        foreach (Material mat in materials)
        {
            RemoveDestination(mat);
        }
    }

    public void SetDestination(Material material)
    {
        material.SetFloat("_Distance", destinationDistance);
    }
    public void RemoveDestination(Material material)
    {
        material.SetFloat("_Distance", recognitionDistance);
    }

    public void StoryUpdated(GlobalData.MainScriptState state)
    {
        switch (state)
        {
            case GlobalData.MainScriptState.START:
                {
                    RemoveAllDestinations();
                    break;
                }
            case GlobalData.MainScriptState.START_MONOLOGUE:
                {
                    SetDestination(DestinationType.CHURCH); 
                    break;
                }
            case GlobalData.MainScriptState.CHURCH_DIALOGUE:
                {
                    RemoveDestination(DestinationType.CHURCH);
                    SetDestination(DestinationType.MARKET); 
                    break;
                }
            case GlobalData.MainScriptState.MARKET:
                {
                    RemoveDestination(DestinationType.MARKET);
                    SetDestination(DestinationType.CAFE);
                    break;
                }
            case GlobalData.MainScriptState.CAFE_FINISHED:
                {
                    RemoveDestination(DestinationType.CAFE);
                    SetDestination(DestinationType.OLD_HOME);
                    break;
                }
            case GlobalData.MainScriptState.OLD_HOME_DIALOGUE:
                {
                    RemoveDestination(DestinationType.OLD_HOME);
                    SetDestination(DestinationType.PARK);
                    break;
                }
            case GlobalData.MainScriptState.NURSERY:
                {
                    RemoveDestination(DestinationType.PARK);
                    SetDestination(DestinationType.NURSERY);
                    break;
                }
        }
    }
}
