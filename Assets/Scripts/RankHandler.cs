using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankHandler : MonoBehaviour
{
    [SerializeField] private Animator _rankAnimator;
    [SerializeField] private GameObject _rankItemPrefab;
    [SerializeField] private Transform _rankItemParent;

    private void ClearAllRankItems()
    {
        foreach (Transform child in _rankItemParent)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void UpdateRank(GamePlayer[] players, ResponsePacketData.EndGame data)
    {
        _rankAnimator.SetTrigger("Open");
        ClearAllRankItems();
        for (var r = 1; r <= data.playerRanks.Length; r++)
        {
            for (var i = 0; i < data.playerRanks.Length; i++)
            {
                if (r == data.playerRanks[i])
                {
                    var rankItem = Instantiate(_rankItemPrefab, _rankItemParent);
                    rankItem.GetComponent<RankItemComponent>().UpdateRankItem(r, players[i].Name, data.playerCards[i], players[i].ColorCode);
                }
            }
        }
    }
}
