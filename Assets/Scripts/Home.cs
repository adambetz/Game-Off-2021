using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Home : MonoBehaviour, IPointerClickHandler
{
    public event Action<GameObject> AntSpawned;

    [SerializeField] private GameObject antPrefab = null;
    [SerializeField] private Transform unitSpawnPoint = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        SpawnAnt();
    }

    private void SpawnAnt()
    {
        GameObject antInstance = Instantiate(antPrefab, unitSpawnPoint.position, unitSpawnPoint.rotation);
        AntSpawned?.Invoke(antInstance);
    }
}
