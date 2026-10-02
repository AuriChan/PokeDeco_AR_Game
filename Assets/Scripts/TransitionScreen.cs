using System.Collections;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] private GameObject decorationMode;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private float waitTime = 4;     
    [SerializeField] private float transitionTime = 1; 
    [SerializeField] private float distance = 1920; 

    void Start()
    {
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
      
        yield return new WaitForSeconds(waitTime);

        audioManager.PlayMusicRand();
        decorationMode.SetActive(true);

        RectTransform rt = (RectTransform)transform;
        Vector2 startPos = rt.anchoredPosition;
        Vector2 endPos = startPos - Vector2.down * distance;

        float elapsed = 0;
        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            rt.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / transitionTime);
            yield return null;   
        }

        rt.anchoredPosition = endPos;
        
        
    }
}