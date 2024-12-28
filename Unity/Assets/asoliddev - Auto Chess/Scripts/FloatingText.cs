using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText: MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Vector3 moveDirection;
    private float timer = 0;

    public float speed = 3;

    public float fadeOutTime = 1f;

    void Update()
    {
        this.transform.position = this.transform.position + moveDirection * speed * Time.deltaTime;

        timer += Time.deltaTime;
        float fade = (fadeOutTime - timer) / fadeOutTime;

        canvasGroup.alpha = fade;

        if (fade <= 0)
            Destroy(this.gameObject);
    }

    public void Init(Vector3 startPosition, float v)
    {
        this.transform.position = startPosition;

        canvasGroup = this.GetComponent<CanvasGroup>();

        this.GetComponent<Text>().text = Mathf.Round(v).ToString();

        moveDirection = new Vector3(Random.Range(-0.5f, 0.5f), 1, Random.Range(-0.5f, 0.5f)).normalized;
    }
}