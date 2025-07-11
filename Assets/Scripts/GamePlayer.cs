using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer
{
    public string Name;
    public int HandCardCount;
    public int ShowCardCount;
    public FruitCard ShowTopCard;

    public GamePlayer(string name, int handCardCount)
    {
        Name = name;
        HandCardCount = handCardCount;
        ShowCardCount = 0;
        ShowTopCard = null;
    }

}
