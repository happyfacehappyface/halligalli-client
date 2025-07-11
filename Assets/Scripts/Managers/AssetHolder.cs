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
}
