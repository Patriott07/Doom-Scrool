using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    // [SerializeField] Animator animator;
    [SerializeField] TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    void OnEnable()
    {
        // animator.Play("play", 0, 0);
        StartCoroutine(Pop());
    }

    IEnumerator Pop()
    {
        canvasGroup.alpha = 0;
        transform.DOMoveY(transform.position.y + 0.4f, 1f);
        canvasGroup.DOFade(1f, 0.8f);

        yield return new WaitForSeconds(2.5f);

        gameObject.SetActive(false);
    }
}
