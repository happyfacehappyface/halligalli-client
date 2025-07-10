using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDrawer : MonoBehaviour
{
    private GameController _controller;
    [SerializeField] private GameObject _fruitCardItemPrefab;
    [SerializeField] private Transform _fruitCardItemParent;

    public void ManualStart(GameController controller)
    {
        _controller = controller;
    }


    private void ClearFruitCardItems()
    {
        foreach (Transform child in _fruitCardItemParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdatePlayers()
    {
    }
    
    public void UpdateFruitCards()
    {
        ClearFruitCardItems();

    }
}
