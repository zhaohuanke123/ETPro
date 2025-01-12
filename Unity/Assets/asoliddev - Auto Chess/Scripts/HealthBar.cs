using UnityEngine;
using UnityEngine.UI;

public class HealthBar: MonoBehaviour
{
    private GameObject championGO;
    private ChampionController championController;
    public Image fillImage;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (championGO != null)
        {
            this.transform.position = championGO.transform.position + new Vector3(0, 1.5f + 1.5f * championGO.transform.localScale.x, 0);
            fillImage.fillAmount = championController.currentHealth / championController.maxHealth;

            if (championController.currentHealth <= 0)
                canvasGroup.alpha = 0;
            else
                canvasGroup.alpha = 1;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Init(GameObject _championGO)
    {
        championGO = _championGO;
        championController = championGO.GetComponent<ChampionController>();
    }
}