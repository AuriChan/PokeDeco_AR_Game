using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{

    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoCapture;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject photoModeUI;
    [SerializeField] private GameObject returnPhotoModeUI;

    [Header("Photo Frames Randomizer")]
    [SerializeField] private Sprite[] frameSprites;
    private Image frameImageComponent;

    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    private Texture2D screenCapture;
    private bool viewingPhoto;

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        returnPhotoModeUI.SetActive(false);

        if (photoFrame != null)
        {
            frameImageComponent = photoFrame.GetComponent<Image>();
        }
    }

    public void TakePhoto()
    {
        if (!viewingPhoto)
        {
            StartCoroutine(CapturePhoto());
        }
        else
        {
            RemovePhoto();
        }
    }

    IEnumerator CapturePhoto()
    {
        photoModeUI.SetActive(false);
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        //save photo into phone gallery
        SaveToGallery();

        ShowPhoto();
        returnPhotoModeUI.SetActive(true);
    }

    void SaveToGallery()
    {
        byte[] bytes = screenCapture.EncodeToPNG();
        string fileName = "PokeDeco_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

        NativeGallery.SaveImageToGallery(bytes, "PokeDecoAR", fileName, (bool success, string path) =>
        {
            Debug.Log("Saved into gallery: " + success + " / Route: " + path);
        });
    }

    IEnumerator CameraFlashEffect()
    {
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }

    void ShowPhoto()
    {
        if (frameSprites != null && frameSprites.Length > 0 && frameImageComponent != null)
        {
            int randomIndex = Random.Range(0, frameSprites.Length);
            frameImageComponent.sprite = frameSprites[randomIndex];
        }

        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;

        photoCapture.SetActive(true);
        StartCoroutine(CameraFlashEffect());
        fadingAnimation.Play("Photo_Fade");
    }

    public void RemovePhoto()
    {
        viewingPhoto = false;
        photoCapture.SetActive(false);

        photoModeUI.SetActive(true);
    }

}

