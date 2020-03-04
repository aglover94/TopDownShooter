using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjective : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DoFade());
    }

    public void StartFade()
    {
        StartCoroutine(DoFade());
    }

    public void ResetFade()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        this.gameObject.SetActive(false);
    }

    public IEnumerator DoFade()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        yield return new WaitForSeconds(2.5f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }
    }
}
