using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedVals : MonoBehaviour
{
    public static LoadedVals instance;
    public int LoadedPesos;
    public int LoadedExperience;
    public int LoadedPotionsCount;
    public int LoadedMaxHP;
    public int LoadedCurrHP;
    public string LoadedSavedMap;
    public bool wasGameLoaded;
    public int LoadedWeaponLevele;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
}
