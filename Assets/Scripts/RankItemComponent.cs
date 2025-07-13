using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankItemComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cardCountText;
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private Image _crownImage;
    [SerializeField] private Image _portraitImage;

    public void UpdateRankItem(int rank, string playerName, int cardCount, int colorIndex)
    {
        _playerNameText.text = playerName;
        _cardCountText.text = cardCount.ToString();

        

        Sprite crownSprite = AssetHolder.Instance.GetCrownSprite(rank);
        if (crownSprite != null)
        {
            _crownImage.sprite = crownSprite;
            _crownImage.gameObject.SetActive(true);
        }
        else
        {
            _crownImage.gameObject.SetActive(false);
        }

        

        _portraitImage.color = AssetHolder.Instance.GetCharacterColor(colorIndex);
    }
}
