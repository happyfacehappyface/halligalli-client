using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PortraitComponent : MonoBehaviour
{
    [SerializeField] private Image _portraitImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _cardLeftText;
    [SerializeField] private Transform _infoTransform;

    [SerializeField] private Image _emotionImage;
    [SerializeField] private Animator _emotionBubbleAnimator;
    private GamePlayer _player;

    public void ManualStart(GamePlayer player)
    {
        _player = player;

        _portraitImage.color = AssetHolder.Instance.GetCharacterColor(_player.ColorCode);
        _nameText.text = _player.Name;

    }
    
    public void AdjustAngle(float angle)
    {   
        _infoTransform.localPosition = (angle >= 180f) ? new Vector3(-160f, 0f, 0f) : new Vector3(160f, 0f, 0f);
    }

    public void UpdateCardLeft()
    {
        _cardLeftText.text = _player.DeckCardCount.ToString();
    }

    public void UpdateEmotion(int emotionID)
    {
        _emotionImage.sprite = AssetHolder.Instance.GetEmotionSprite(emotionID);
        _emotionBubbleAnimator.SetTrigger("Popup");
    }

}
