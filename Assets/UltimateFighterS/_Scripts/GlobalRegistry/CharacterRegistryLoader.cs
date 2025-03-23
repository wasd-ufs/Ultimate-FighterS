using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterRegistryLoader : MonoBehaviour
{
    private static bool _isFirstLoad = true;
    public static readonly UnityEvent OnLoadComplete = new();
    [SerializeField] private List<Character> charactersToRegister;

    private void Start()
    {
        if (_isFirstLoad)
        {
            CharacterRegistry.RegisterRange(charactersToRegister);
            _isFirstLoad = false;
        }

        OnLoadComplete.Invoke();
    }
}