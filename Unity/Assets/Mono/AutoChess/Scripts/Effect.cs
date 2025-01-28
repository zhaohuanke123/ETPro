using UnityEngine;

public class Effect: MonoBehaviour
{
    public GameObject effectPrefab;

    public float duration;
    private GameObject championGO;
    private GameObject effectGO;

    private void Update()
    {
        duration -= Time.deltaTime;

        if (duration < 0)
            championGO.GetComponent<ChampionController>().RemoveEffect(this);
    }

    public void Init(GameObject _effectPrefab, GameObject _championGO, float _duration)
    {
        effectPrefab = _effectPrefab;
        duration = _duration;
        championGO = _championGO;

        effectGO = Instantiate(effectPrefab, championGO.transform, true);
        effectGO.transform.localPosition = Vector3.zero;
    }

    public void Remove()
    {
        Destroy(effectGO);
        Destroy(this);
    }
}