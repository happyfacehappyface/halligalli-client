using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetHolder : MonoBehaviour
{
    public static AssetHolder Instance;
    [SerializeField] private Sprite[] _fruitSprites;

    private bool _isReady = false;

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
}
