using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasController: MonoBehaviour
{
    public GameObject worldCanvas;
    public GameObject floatingTextPrefab;
    public GameObject healthBarPrefab;

    public void AddDamageText(Vector3 position, float v)
    {
        GameObject go = Instantiate(floatingTextPrefab);
        go.transform.SetParent(worldCanvas.transform);

        go.GetComponent<FloatingText>().Init(position, v);
    }

    public void AddHealthBar(GameObject championGO)
    {
        GameObject go = Instantiate(healthBarPrefab);
        go.transform.SetParent(worldCanvas.transform);

        go.GetComponent<HealthBar>().Init(championGO);
    }
}