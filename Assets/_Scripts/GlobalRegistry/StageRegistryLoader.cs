using System.Collections.Generic;
using UnityEngine;

public class StageRegistryLoader : MonoBehaviour
{
    private static bool isFirstLoad = true;
    
    [SerializeField] private List<Stage> stagesToRegister;
    
    private void Awake()
    {
        if (isFirstLoad)
        {
            StageRegistry.RegisterRange(stagesToRegister);
            isFirstLoad = false;
        }
        
        Destroy(this);
    }
}