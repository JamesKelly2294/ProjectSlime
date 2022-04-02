using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ResourceType
{
    // NOTE: If you add or remove to this, do *not* change the values of other
    // enums. This will break all serialized scriptable objects.
    Money = 0,
    Energy = 1,
    CleanEnergy = 2,
    DirtyEnergy = 3,
    LowTechMat = 4,
    HighTechMat = 5,
    Biomass = 6,
    Research = 7
}

[System.Serializable]
public class ResourceEffect
{
    [SerializeField]
    ResourceType AffectedResource;

    [Range(-10, 10)]
    public int EffectAmount;
}

[System.Serializable]
public class SpecialResourceEffect
{
    public enum SpecialResourceEffectType
    {
        // NOTE: If you add or remove to this, do *not* change the values of other
        // enums. This will break all serialized scriptable objects.
        EmptyBuilding = 0,
        Test = 1,
    }

    public string Name;
    public string Description;

    [SerializeField]
    SpecialResourceEffectType Type;
}

[CreateAssetMenu(fileName = "Building", menuName = "Game/Building", order = 1)]
public class Building : ScriptableObject
{
    public string Name;
    public string Description;
    public GameObject VisualsPrefab;
    public Sprite PreviewImage;

    public bool Buildable = true;
    public bool Disableable = true;
    public bool Deconstructable = true;

    public int MoneyCost;
    public int PopCost;

    public List<ResourceEffect> ResourceEffects;
    public List<SpecialResourceEffect> SpecialEffects;
}
