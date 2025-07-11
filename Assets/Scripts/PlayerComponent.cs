using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _handCardCountText;
    [SerializeField] private TextMeshProUGUI _showCardCountText;
    [SerializeField] private FruitCardItemComponent _fruitCardItem;

    private GamePlayer _player;

    public void ManualStart(GamePlayer player)
    {
        _player = player;
        UpdatePlayer();
    }

    public void UpdatePlayer()
    {
        _nameText.text = _player.Name;
        _handCardCountText.text = _player.HandCardCount.ToString();
        _showCardCountText.text = _player.ShowCardCount.ToString();
        _fruitCardItem.UpdateFruitCard(_player.ShowTopCard, true);
    }


}
