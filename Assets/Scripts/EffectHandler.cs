using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private GameObject _bellHitEffect;
    [SerializeField] private Transform _effectParent;

    private List<EffectItem> _effectObjects;

    private TimeSpan _effectTime;
    private GameDrawer _gameDrawer;
    public void ManualStart(GameDrawer gameDrawer)
    {
        _effectTime = TimeSpan.Zero;
        _effectObjects = new List<EffectItem>();
        _gameDrawer = gameDrawer;
    }

    public void ManualUpdate()
    {
        _effectTime = _effectTime + TimeSpan.FromSeconds(Time.deltaTime);

        for (var i = _effectObjects.Count - 1; i >= 0; i--)
        {
            if (_effectTime >= _effectObjects[i].effectEndTime)
            {
                Destroy(_effectObjects[i].effectObject);
                _effectObjects.RemoveAt(i);
            }
        }

    }

    public void AddBellHitEffect(Vector3 localPosition, Quaternion localRotation)
    {
        var effectObject = Instantiate(_bellHitEffect, _effectParent);
        _effectObjects.Add(new EffectItem(effectObject, _effectTime + TimeSpan.FromSeconds(1.0f)));
        effectObject.transform.localPosition = localPosition;
        effectObject.transform.localRotation = localRotation;
        effectObject.GetComponent<ParticleSystem>().Play();
    }


    private sealed record EffectItem(GameObject effectObject, TimeSpan effectEndTime);
    
}
