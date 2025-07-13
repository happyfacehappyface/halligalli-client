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
    [SerializeField] private Transform _emotionBubbleRootTransform;
    [SerializeField] private Transform _emotionBubbleTransform;
    [SerializeField] private Image _crownImage;
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
        
        if (angle <= 170f && angle >= 190f)
        {
            _emotionBubbleTransform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            _emotionBubbleRootTransform.localPosition = new Vector3(-100f, -100f, 0f);
        }
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


    /*
    public void OnPlayerRankChanged(int rank)
    {
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
    }
    */

}
