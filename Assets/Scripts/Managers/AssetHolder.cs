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
        if (colorCode == 0) return new Color(251f/255f, 197f/255f, 96f/255f);
        if (colorCode == 1) return new Color(236f/255f, 230f/255f, 220f/255f);
        if (colorCode == 2) return new Color(53f/255f, 53f/255f, 53f/255f);
        if (colorCode == 3) return new Color(123f/255f, 123f/255f, 123f/255f);
        if (colorCode == 4) return new Color(106f/255f, 66f/255f, 22f/255f);
        if (colorCode == 5) return new Color(96f/255f, 138f/255f, 142f/255f);
        if (colorCode == 6) return new Color(241f/255f, 178f/255f, 233f/255f);
        if (colorCode == 7) return new Color(231f/255f, 248f/255f, 158f/255f);
        return Color.white;
    }
}
