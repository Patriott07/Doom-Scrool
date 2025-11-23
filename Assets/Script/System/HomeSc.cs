using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSc : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartClick()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void SettingClick()
    {
        SceneManager.LoadScene("OptionUI");
    }

    public void CreditClick()
    {
        SceneManager.LoadScene("CreditUI");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
