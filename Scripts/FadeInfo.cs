using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInfo : MonoBehaviour
{
    //Public method used to start the DoFade coroutine
    public void StartFade()
    {
        StartCoroutine(DoFade());
    }

    //Public method used to reset the fade
    public void ResetFade()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        this.gameObject.SetActive(false);
    }

    //Start this coroutine when player tries to pick up health or ammo when at full health or ammo
    public IEnumerator DoFade()
    {
        //Get the CanvasGroup component from the object and set it to the canvasGroup variable
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        //Wait for 2.5 seconds before continuing on
        yield return new WaitForSeconds(2.5f);

        //While the alpha value on canvasGroup is more than 0 minus that value by Time.deltaTime / 2
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }

        //Call the ResetFade method
        ResetFade();
    }
}
