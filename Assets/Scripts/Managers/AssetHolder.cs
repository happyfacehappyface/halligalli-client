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

    public Color GetCharacterColor(int characterID)
    {
        if (characterID == 0) return new Color(1.0f, 1.0f, 1.0f);
        if (characterID == 1) return new Color(0.7f, 0.7f, 0.7f);
        if (characterID == 2) return new Color(0.4f, 0.4f, 0.4f);
        return Color.white;
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
}
