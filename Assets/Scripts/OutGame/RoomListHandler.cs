using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListHandler : MonoBehaviour
{
    [SerializeField] private GameObject _roomItemPrefab;
    [SerializeField] private Transform _roomItemParent;

    private OutGameController _controller;

    public void ManualStart(OutGameController controller)
    {
        _controller = controller;
    }

    private void ClearRoomList()
    {
        foreach (Transform child in _roomItemParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateRoomList(List<RoomInfo> roomInfos)
    {
        ClearRoomList();

        foreach (var roomInfo in roomInfos)
        {
            GameObject roomItem = Instantiate(_roomItemPrefab, _roomItemParent);
            roomItem.GetComponent<RoomItemComponent>().UpdateRoomItem(_controller, roomInfo);
        }
    }

}




