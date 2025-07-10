using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetHolder : MonoBehaviour
{
    [SerializeField] private Sprite[] _fruitSprites;

    public Sprite GetFruitSprite(int fruitID)
    {
        return _fruitSprites[fruitID];
    }
}
