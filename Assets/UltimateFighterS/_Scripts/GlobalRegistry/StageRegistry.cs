using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StageRegistry
{
    private static readonly Dictionary<GameObject, Stage> StageSet = new();
    
    public static List<Stage> Stages => StageSet.Values.ToList();
    public static int StageCount => StageSet.Count;
    
    public static void Register(Stage character) => StageSet.TryAdd(character.prefab, character);

    public static void RegisterRange(IEnumerable<Stage> stages)
    {
        foreach (var stage in stages)
            Register(stage);
    }
}

[Serializable]
public class Stage
{
    public string name;
    public Texture icon;
    public GameObject prefab;
}