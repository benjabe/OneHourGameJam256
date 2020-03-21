using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MoonSharp.Interpreter.MoonSharpUserData]
public class Gravity
{
    public static float Strength { get; set; }
    public static float HorizontalStrength { get; set; }
    public static void Flip()
    {
        Strength = -Strength;
    }
    public static void FlipHorizontal()
    {
        HorizontalStrength = -HorizontalStrength;
    }
}
