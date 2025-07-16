using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private FruitCardItemComponent _topCardItem;
    [SerializeField] private FruitCardItemComponent _deckCardItem;
    [SerializeField] private FruitCardItemComponent _showCardItem;

    private GamePlayer _player;

    public void ManualStart(GamePlayer player)
    {
        _player = player;
        UpdatePlayer();
    }

    public void UpdatePlayer()
    {

        if (_player.ShowCardCount > 0)
        {
            _topCardItem.gameObject.SetActive(true);
            _topCardItem.UpdateFaceUp(_player.ShowTopCard);
        }
        else
        {
            _topCardItem.gameObject.SetActive(false);
        }

        if (_player.DeckCardCount > 0)
        {
            _deckCardItem.gameObject.SetActive(true);
            _deckCardItem.UpdateFaceDown();
        }
        else
        {
            _deckCardItem.gameObject.SetActive(false);
        }

        _showCardItem.gameObject.SetActive(false);
    }

    public void UpdateWithFlipCard()
    {

        if (_player.ShowCardCount > 1)
        {
            _topCardItem.gameObject.SetActive(true);
            _topCardItem.UpdateFaceUp(_player.ShowSecondCard);
        }
        else
        {
            _topCardItem.gameObject.SetActive(false);
        }

        if (_player.DeckCardCount > 0)
        {
            _deckCardItem.gameObject.SetActive(true);
            _deckCardItem.UpdateFaceDown();
        }
        else
        {
            _deckCardItem.gameObject.SetActive(false);
        }

        _showCardItem.gameObject.SetActive(true);
        _showCardItem.UpdateFlip(_player.ShowTopCard);
    }


}
