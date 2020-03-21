using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBInputUpdater : MonoBehaviour
{
    private void Update()
    {
        BBInput.Eaten = new List<KeyCode>();
        BBInput.OnEatenKeysReset?.Invoke();

        void Handle(SortedDictionary<int, Dictionary<KeyCode, Action>> keyEvents, Func<KeyCode, bool> isTriggered)
        {
            var orders = new List<int>();
            foreach (var key in keyEvents.Keys)
            {
                orders.Add(key);
            }
            foreach (var order in orders)
            {
                var keyCodes = new List<KeyCode>();
                foreach (var keyCode in keyEvents[order].Keys)
                {
                    keyCodes.Add(keyCode);
                }
                foreach (var keyCode in keyCodes)
                {
                    if (!BBInput.Eaten.Contains(keyCode) && isTriggered(keyCode))
                    {
                        keyEvents[order][keyCode]?.Invoke();
                    }
                }
            }
        }
        Handle(BBInput.OnKeyDown, Input.GetKeyDown);
        Handle(BBInput.OnKeyUp, Input.GetKeyUp);
        Handle(BBInput.OnKey, Input.GetKey);
    }
}
