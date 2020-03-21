using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A very basic input wrapper that allows for event-driven input
/// with the ability to prioritise listeners and which gives listeners the ability to eat input.
/// </summary>
public class BBInput
{
    private static BBInputUpdater _updater;

    private static List<KeyCode> _keys = new List<KeyCode>();

    public static SortedDictionary<int, Dictionary<KeyCode, Action>> OnKeyDown =
        new SortedDictionary<int, Dictionary<KeyCode, Action>>();
    public static SortedDictionary<int, Dictionary<KeyCode, Action>> OnKeyUp =
        new SortedDictionary<int, Dictionary<KeyCode, Action>>();
    public static SortedDictionary<int, Dictionary<KeyCode, Action>> OnKey =
        new SortedDictionary<int, Dictionary<KeyCode, Action>>();

    public static List<KeyCode> Eaten = new List<KeyCode>();
    public static Action OnEatenKeysReset;

    private static void Add(
        SortedDictionary<int, Dictionary<KeyCode, Action>> keyEvents,
        KeyCode keyCode,
        Action action,
        int order)
    {
        if (!keyEvents.ContainsKey(order))
        {
            keyEvents[order] = new Dictionary<KeyCode, Action>();
        }
        if (!keyEvents[order].ContainsKey(keyCode))
        {
            keyEvents[order].Add(keyCode, null);
        }
        keyEvents[order][keyCode] += action;

        if (!_keys.Contains(keyCode))
        {
            _keys.Add(keyCode);
        }

        if (_updater == null)
        {
            _updater = GameObject.Instantiate(new GameObject()).AddComponent<BBInputUpdater>();
            GameObject.DontDestroyOnLoad(_updater);
        }
    }

    public static void AddOnKeyDown(KeyCode keyCode, Action action, int order = 0)
    {
        Add(OnKeyDown, keyCode, action, order);
    }
    public static void AddOnKeyDown(string key, Action action, int order = 0)
    {
        Add(OnKeyDown, (KeyCode)Enum.Parse(typeof(KeyCode), key), action, order);
    }

    public static void AddOnKeyUp(KeyCode keyCode, Action action, int order = 0)
    {
        Add(OnKeyUp, keyCode, action, order);
    }
    public static void AddOnKeyUp(string key, Action action, int order = 0)
    {
        Add(OnKeyUp, (KeyCode)Enum.Parse(typeof(KeyCode), key), action, order);
    }

    public static void AddOnKey(KeyCode keyCode, Action action, int order = 0)
    {
        Add(OnKey, keyCode, action, order);
    }
    public static void AddOnKey(string key, Action action, int order = 0)
    {
        Add(OnKey, (KeyCode)Enum.Parse(typeof(KeyCode), key), action, order);
    }

    public static void EatAll()
    {
        Eaten.AddRange(_keys);
    }

    public static void UnEat(KeyCode keyCode)
    {
        Eaten.Remove(keyCode);
    }
}