using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static FruitCard GetRandomFruitCard()
    {
        return new FruitCard(Random.Range(0, 3), Random.Range(1, 6));
    }


    public static int RealModulo(int a, int b)
    {
        return (a % b + b) % b;
    }


    public static void Log(string message)
    {
        #if UNITY_EDITOR
        Debug.Log(message);
        #endif
    }

    public static void LogWarning(string message)
    {
        #if UNITY_EDITOR
        Debug.LogWarning(message);
        #endif
    }

    public static void LogError(string message)
    {
        #if UNITY_EDITOR
        Debug.LogError(message);
        #endif
    }


}
