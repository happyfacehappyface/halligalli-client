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
    [SerializeField] private Sprite _goldCrownSprite;
    [SerializeField] private Sprite _silverCrownSprite;
    [SerializeField] private Sprite _bronzeCrownSprite;

    public void UpdateRankItem(int rank, string playerName, int cardCount, int colorIndex)
    {
        _playerNameText.text = playerName;
        _cardCountText.text = cardCount.ToString();

        if (rank == 1)
        {
            _crownImage.gameObject.SetActive(true);
            _crownImage.sprite = _goldCrownSprite;
        }
        else if (rank == 2)
        {
            _crownImage.gameObject.SetActive(true);
            _crownImage.sprite = _silverCrownSprite;
        }
        else if (rank == 3)
        {
            _crownImage.gameObject.SetActive(true);
            _crownImage.sprite = _bronzeCrownSprite;
        }
        else
        {
            _crownImage.gameObject.SetActive(false);
        }

        _portraitImage.color = AssetHolder.Instance.GetCharacterColor(colorIndex);
    }
}
