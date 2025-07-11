using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class FruitCardItemComponent : MonoBehaviour
{
    [SerializeField] private GameObject _card;
    [SerializeField] private Animator _cardAnimator;
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

    public void AnimateFaceUp()
    {
        _cardAnimator.SetTrigger("FaceUp");
    }

    public void AnimateFaceDown()
    {

    }

    public void AnimateFlip()
    {
        _cardAnimator.SetTrigger("Flip");
    }

    public void UpdateFaceUp(FruitCard fruitCard)
    {
        SetFruit(fruitCard.FruitID, fruitCard.Count);

        AnimateFaceUp();
    }

    public void UpdateFaceDown()
    {
        AnimateFaceDown();
    }

    public void UpdateFlip(FruitCard fruitCard)
    {
        SetFruit(fruitCard.FruitID, fruitCard.Count);
        AnimateFlip();
    }

    private void SetFruitType(int fruitID)
    {
        _fruitCenter.sprite = AssetHolder.Instance.GetFruitSprite(fruitID);
        _fruitImageTopLeft.sprite = AssetHolder.Instance.GetFruitSprite(fruitID);
        _fruitImageTopRight.sprite = AssetHolder.Instance.GetFruitSprite(fruitID);
        _fruitImageBottomLeft.sprite = AssetHolder.Instance.GetFruitSprite(fruitID);
        _fruitImageBottomRight.sprite = AssetHolder.Instance.GetFruitSprite(fruitID);
    }

    private void SetFruitCount(int count)
    {
        _fruitCenter.gameObject.SetActive(new int[] {1, 3, 5}.Contains(count));
        _fruitImageTopLeft.gameObject.SetActive(new int[] {2, 3, 4, 5}.Contains(count));
        _fruitImageTopRight.gameObject.SetActive(new int[] {4, 5}.Contains(count));
        _fruitImageBottomLeft.gameObject.SetActive(new int[] {4, 5}.Contains(count));
        _fruitImageBottomRight.gameObject.SetActive(new int[] {2, 3, 4, 5}.Contains(count));
    }

    private void SetFruit(int fruitID, int count)
    {
        SetFruitType(fruitID);
        SetFruitCount(count);
    }

    
}
