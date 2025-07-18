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

    
    public static float GetLinearProgress(float start, float end, float progress)
    {
        if (start == end) return 0f;
        return Mathf.Clamp01((progress - start) / (end - start));
    }

    public static float GetEaseInProgress(float start, float end, float progress)
    {
        if (start == end) return 0f;
        float linearProgress = GetLinearProgress(start, end, progress);
        return linearProgress * linearProgress;
    }

    public static float GetEaseOutProgress(float start, float end, float progress)
    {
        if (start == end) return 0f;
        float linearProgress = GetLinearProgress(start, end, progress);
        return 1f - (1f - linearProgress) * (1f - linearProgress);
    }

    public static float GetEaseInOutProgress(float start, float end, float progress)
    {
        if (start == end) return 0f;
        float linearProgress = GetLinearProgress(start, end, progress);
        return linearProgress < 0.5f ? 2f * linearProgress * linearProgress : 1f - Mathf.Pow(-2f * linearProgress + 2f, 2f) / 2f;
    }


    public static string GetFruitVariationDescription(int fruitVariation)
    {
        return $"{fruitVariation}종류의\n과일이\n등장합니다!";
    }

    public static string GetFruitCountDescription(int fruitCount)
    {
        return $"{fruitCount}개의 같은\n과일이 보이면\n종을 치세요!";
    }

    public static string GetGameTempoDescription(int gameTempo)
    {
        string description = string.Empty;
        if (gameTempo == 0) description = "천천히";
        if (gameTempo == 1) description = "일반적인 속도로";
        if (gameTempo == 2) description = "빠르게";
        if (gameTempo > 3) description = "매우 빠르게";
        return $"카드가\n{description}\n공개됩니다!";
    }


}
