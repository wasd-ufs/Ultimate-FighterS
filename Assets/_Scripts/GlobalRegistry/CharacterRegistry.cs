using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CharacterRegistry
{
    private static readonly Dictionary<GameObject, Character> CharacterSet = new();
    
    public static List<Character> Characters => CharacterSet.Values.ToList();
    public static int CharacterCount => CharacterSet.Count;
    
    public static void Register(Character character) => CharacterSet.TryAdd(character.prefab, character);

    public static void RegisterRange(IEnumerable<Character> characters)
    {
        foreach (var character in characters)
            Register(character);
    }
}

[Serializable]
public class Character
{
    public string name;
    public Texture icon;
    public GameObject prefab;
}