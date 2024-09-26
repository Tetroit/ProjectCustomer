using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeIn : MonoBehaviour
{
    public Image[] images;
    public float fadeDuration = 1.0f;
    public float delayBetweenImages = 0.5f;
    bool cutsceneEnded = false;
    public GameObject hint;

    void Start()
    {
        if (hint != null)
            hint.SetActive(false);
        StartCoroutine(FadeInImages());
    }

    private void Update()
    {
        if (cutsceneEnded)
        {
            if (hint != null)
                hint.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameBrain.main.LoadGame();
            }
        }
    }

    IEnumerator FadeInImages()
    {
        foreach (Image image in images)
        {
            yield return StartCoroutine(FadeIn(image));
            yield return new WaitForSeconds(delayBetweenImages);
        }
        CutsceneEnd();
    }

    IEnumerator FadeIn(Image image)
    {
        Color originalColor = image.color;
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    public void CutsceneEnd()
    {
        SaveManager.instance.gameData.isCutscenePlayed = true;
        GlobalData.instance.isCutscenePlayed = true;
        cutsceneEnded = true;
    }
}