using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingUI : MonoBehaviour
{
    // Ganti string ini sesuai nama Scene Loading Anda di Unity
    private const string LOADING_SCENE_NAME = "LoadingScreen"; 
    
    // Variabel statis untuk menyimpan scene tujuan antar-scene
    private static string targetSceneName;

    [Header("UI Elements")]
    [SerializeField] private GameObject loadingScreenPanel; 
    [SerializeField] private RawImage loadingImage;
    [SerializeField] private Slider progressBar; 
    [SerializeField] private Text progressText; 

    [Header("Floating Animation")]
    [SerializeField] private float floatAmplitude = 10f;
    [SerializeField] private float floatFrequency = 1f;

    private Vector2 startPosition;
    private RectTransform rectTransform;

    // --- FUNGSI UTAMA ---
    // Panggil fungsi ini dari script lain: LoadingUI.LoadScene("NamaScene");
    public static void LoadScene(string sceneName)
    {
        targetSceneName = sceneName;
        SceneManager.LoadScene(LOADING_SCENE_NAME);
    }

    private void Start()
    {
        // Setup Animasi
        if (loadingImage == null) loadingImage = GetComponentInChildren<RawImage>();

        if (loadingImage != null)
        {
            rectTransform = loadingImage.rectTransform;
            startPosition = rectTransform.anchoredPosition;
        }

        // Pastikan panel aktif
        if (loadingScreenPanel != null) loadingScreenPanel.SetActive(true);

        // Mulai loading scene tujuan
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            StartCoroutine(LoadSceneAsync(targetSceneName));
        }
        else
        {
            // Debugging jika scene ini di-play langsung tanpa lewat LoadScene()
            Debug.LogWarning("LoadingUI: Tidak ada target scene. Membuka scene ini secara langsung?");
        }
    }

    private void Update()
    {
        // Animasi Floating
        if (rectTransform != null)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            rectTransform.anchoredPosition = new Vector2(startPosition.x, newY);
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Delay kecil agar loading screen tidak cuma kedip (opsional)
        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        // Mencegah scene langsung aktif sampai loading selesai (opsional)
        // operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Progress value Unity adalah 0 sampai 0.9
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null) progressBar.value = progress;
            if (progressText != null) progressText.text = (progress * 100f).ToString("F0") + "%";

            yield return null;
        }
    }
}
