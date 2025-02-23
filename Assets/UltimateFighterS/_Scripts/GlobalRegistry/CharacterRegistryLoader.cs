using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterRegistryLoader : MonoBehaviour
{
    [SerializeField] private List<Character> charactersToRegister;
    
    private static bool isFirstLoad = true;
    public static readonly UnityEvent OnLoadComplete = new();
    
    private void Start()
    {
        if (isFirstLoad)
        {
            CharacterRegistry.RegisterRange(charactersToRegister);
            isFirstLoad = false;
        }
        
        OnLoadComplete.Invoke();
    }
}