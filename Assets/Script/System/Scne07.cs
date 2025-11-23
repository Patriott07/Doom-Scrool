using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scne07 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(HomeBack());
    }

    IEnumerator HomeBack()
    {
        PlayerPrefs.DeleteKey("mood");
        yield return new WaitForSeconds(20f);
        SceneManager.LoadScene("Home");
    }
}
