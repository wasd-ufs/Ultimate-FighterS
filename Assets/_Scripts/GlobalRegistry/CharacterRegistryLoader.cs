using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRegistryLoader : MonoBehaviour
{
    private static bool isFirstLoad = true;
    
    [SerializeField] private List<Character> charactersToRegister;
    
    private void Awake()
    {
        if (isFirstLoad)
        {
            CharacterRegistry.RegisterRange(charactersToRegister);
            isFirstLoad = false;
        }
        
        Destroy(this);
    }
}