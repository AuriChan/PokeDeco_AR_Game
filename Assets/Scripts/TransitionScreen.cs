using System.Collections;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private float waitTime = 4;     
    [SerializeField] private float transitionTime = 1; 
    [SerializeField] private AudioClip PhotoMusic;

    private RectTransform rect;
    private Vector2 onScreenPos;
    void Awake()
    {
        rect = (RectTransform)transform;
        onScreenPos = rect.anchoredPosition;
    }
    public void StartTransition(GameObject mode)
    {
        StartCoroutine(TransitionRoutine(mode));
    }
    public IEnumerator TransitionRoutine(GameObject mode)
    {

        rect.anchoredPosition = onScreenPos;
        yield return new WaitForSeconds(waitTime);

        mode.SetActive(true);
        if (mode.name == "Decoration_Mode")
        {
            audioManager.PlayMusicRand();
        }
        else if (mode.name == "Photo_Mode")
        {
            audioManager.PlayMusic(PhotoMusic);
        }

        RectTransform canvasRect = rect.root.GetComponent<RectTransform>();
        float distance = canvasRect.rect.height + rect.rect.height;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + Vector2.up * distance;

        float elapsed = 0;
        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionTime;
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        rect.anchoredPosition = endPos;


    }
}