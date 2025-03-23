using System.Collections.Generic;
using UnityEngine;

public class StageRegistryLoader : MonoBehaviour
{
    private static bool _isFirstLoad = true;

    [SerializeField] private List<Stage> stagesToRegister;

    private void Awake()
    {
        if (_isFirstLoad)
        {
            StageRegistry.RegisterRange(stagesToRegister);
            _isFirstLoad = false;
        }

        Destroy(this);
    }
}