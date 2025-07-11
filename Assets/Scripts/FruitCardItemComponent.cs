using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class FruitCardItemComponent : MonoBehaviour
{
    [SerializeField] private GameObject _card;
    [SerializeField] private GameObject _cardBack;
    [SerializeField] private Image _fruitCenter;
    [SerializeField] private Image _fruitImageTopLeft;
    [SerializeField] private Image _fruitImageTopRight;
    [SerializeField] private Image _fruitImageBottomLeft;
    [SerializeField] private Image _fruitImageBottomRight;

    private GameController _controller;

    public void ManualStart(GameController controller)
    {
        _controller = controller;
    }

    public void UpdateFruitCard(FruitCard fruitCard, bool isFaceUp)
    {
        if (fruitCard == null)
        {
            _card.SetActive(false);
            return;
        }

        _card.SetActive(true);

        _cardBack.SetActive(!isFaceUp);

        Utils.Log($"FruitCardItemComponent.UpdateFruitCard: {fruitCard.FruitID}, {fruitCard.Count}");

        _fruitCenter.sprite = AssetHolder.Instance.GetFruitSprite(fruitCard.FruitID);
        _fruitImageTopLeft.sprite = AssetHolder.Instance.GetFruitSprite(fruitCard.FruitID);
        _fruitImageTopRight.sprite = AssetHolder.Instance.GetFruitSprite(fruitCard.FruitID);
        _fruitImageBottomLeft.sprite = AssetHolder.Instance.GetFruitSprite(fruitCard.FruitID);
        _fruitImageBottomRight.sprite = AssetHolder.Instance.GetFruitSprite(fruitCard.FruitID);
        
        _fruitCenter.gameObject.SetActive(new int[] {1, 3, 5}.Contains(fruitCard.Count));
        _fruitImageTopLeft.gameObject.SetActive(new int[] {2, 3, 4, 5}.Contains(fruitCard.Count));
        _fruitImageTopRight.gameObject.SetActive(new int[] {4, 5}.Contains(fruitCard.Count));
        _fruitImageBottomLeft.gameObject.SetActive(new int[] {4, 5}.Contains(fruitCard.Count));
        _fruitImageBottomRight.gameObject.SetActive(new int[] {2, 3, 4, 5}.Contains(fruitCard.Count));



    }

    
}
