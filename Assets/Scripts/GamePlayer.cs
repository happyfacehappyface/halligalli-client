using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer
{
    public string Name;
    public int DeckCardCount;
    public int ShowCardCount;
    public FruitCard ShowTopCard;
    public FruitCard ShowSecondCard;
    public int ColorCode;

    public GamePlayer(string name, int handCardCount, int colorCode)
    {
        Name = name;
        DeckCardCount = handCardCount;
        ShowCardCount = 0;
        ShowTopCard = null;
        ShowSecondCard = null;
        ColorCode = colorCode;
    }

    public void ChangeTopCard(FruitCard card)
    {
        
        ShowSecondCard = ShowTopCard;
        ShowTopCard = card;

        ShowCardCount++;
        DeckCardCount--;

    }

}
