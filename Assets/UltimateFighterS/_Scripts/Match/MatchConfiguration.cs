using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchConfiguration
{
    public static GameObject ScenePrefab = null;
    public static Dictionary<int, Character> Characters = new();
    public static Dictionary<int, InputType> PlayerInputTypes = new();
}

public enum InputType
{
    Player,
    NoInput,
}