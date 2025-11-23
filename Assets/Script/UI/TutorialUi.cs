using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TutorialUi : MonoBehaviour
{
    [Header("Tutorial Settings")]
    [SerializeField] private List<RawImage> tutorialImages; // List gambar tutorial (RawImage)
    [SerializeField] private string gameSceneName = "LoadingScreen"; // Nama scene permainan

    private int currentIndex = 0;

    private void Start()
    {
        if (tutorialImages == null || tutorialImages.Count == 0)
        {
            Debug.LogWarning("TutorialUi: List gambar tutorial kosong!");
            return;
        }

        // Reset: Sembunyikan semua gambar dulu
        foreach (var img in tutorialImages)
        {
            if (img != null) img.gameObject.SetActive(false);
        }

        // Tampilkan gambar pertama
        ShowImage(currentIndex);
    }

    private void Update()
    {
        // Cek input Spasi
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextImage();
        }
    }

    public void NextImage()
    {
        if (tutorialImages.Count == 0) return;

        // Sembunyikan gambar saat ini
        if (currentIndex < tutorialImages.Count && tutorialImages[currentIndex] != null)
        {
            tutorialImages[currentIndex].gameObject.SetActive(false);
        }

        currentIndex++;

        // Jika masih ada gambar selanjutnya
        if (currentIndex < tutorialImages.Count)
        {
            ShowImage(currentIndex);
        }
        else
        {
            // Tutorial selesai
            OnTutorialComplete();
        }
    }

    private void ShowImage(int index)
    {
        if (index >= 0 && index < tutorialImages.Count && tutorialImages[index] != null)
        {
            tutorialImages[index].gameObject.SetActive(true);
        }
    }

    private void OnTutorialComplete()
    {
        Debug.Log("Tutorial Selesai! Memuat scene: " + gameSceneName);
        LoadingUI.LoadScene(gameSceneName);
    }
}
