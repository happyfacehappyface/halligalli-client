using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetHolder : MonoBehaviour
{
    public static AssetHolder Instance;
    [SerializeField] private Sprite[] _fruitSprites;
    [SerializeField] private Sprite[] _emotionSprites;

    [SerializeField] private Sprite _goldCrownSprite;
    [SerializeField] private Sprite _silverCrownSprite;
    [SerializeField] private Sprite _bronzeCrownSprite;

    private bool _isReady = false;
    public bool IsReady() => _isReady;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _isReady = true;
    }
    public Sprite GetFruitSprite(int fruitID)
    {
        return _fruitSprites[fruitID];
    }


    public Sprite GetEmotionSprite(int emotionID)
    {
        return _emotionSprites[emotionID];
    }

    public Sprite GetCrownSprite(int rank)
    {
        if (rank == 1) return _goldCrownSprite;
        if (rank == 2) return _silverCrownSprite;
        if (rank == 3) return _bronzeCrownSprite;
        return null;
    }

    public Color GetCharacterColor(int colorCode)
    {
        if (colorCode == 0) return new Color(1.0f, 1.0f, 1.0f);
        if (colorCode == 1) return new Color(0.7f, 0.7f, 0.7f);
        if (colorCode == 2) return new Color(0.4f, 0.4f, 0.4f);
        if (colorCode == 3) return new Color(0.2f, 0.2f, 0.2f);
        if (colorCode == 4) return new Color(0.1f, 0.1f, 0.1f);
        if (colorCode == 5) return new Color(0.0f, 0.0f, 0.0f);
        if (colorCode == 6) return new Color(0.0f, 0.0f, 0.0f);
        if (colorCode == 7) return new Color(0.0f, 0.0f, 0.0f);
        if (colorCode == 8) return new Color(0.0f, 0.0f, 0.0f);
        return Color.white;
    }
}
