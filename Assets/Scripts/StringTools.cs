using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringTools 
{
    public static string FirstCharToUpper(string str) => (str.Length > 2) ? str[0].ToString().ToUpper() + str.Substring(1) : str;
    public static float Hectogram2Kilogram(string weight) => (weight.Length > 0) ? (float.Parse(weight) * 0.1f) : 1;
    public static float Decimeter2Meter(string height) => (height.Length > 0) ? (float.Parse(height) * 0.1f) : 1;

}
