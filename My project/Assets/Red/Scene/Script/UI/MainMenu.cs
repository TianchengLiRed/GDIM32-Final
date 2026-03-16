using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image startImage;
    public float fadeDuration = 1f;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject startText;
    [SerializeField] private TextMeshProUGUI emailText;
    [SerializeField] private float typeSpeed = 0.03f;
    // Start is called before the first frame update
    void Start()
    {
        startImage.gameObject.SetActive(true);
        Color c = startImage.color;
        c.a = 0f;
        startImage.color = c;
        StartCoroutine(FadeIn());
        startPanel.SetActive(true);
        startText.SetActive(true); 
        StartCoroutine(TypeEmail());  
    }

    // Update is called once per frame
    void Update()
    {
        bool keyInput = Input.anyKeyDown;
        bool mosueInput = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
        if(keyInput||mosueInput)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        startImage.gameObject.SetActive(false);
        startPanel.SetActive(false);
        startText.SetActive(false);   
        emailText.gameObject.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            Color c = startImage.color;
            c.a = t / fadeDuration;
            startImage.color = c;

            yield return null;
        }

        Color final = startImage.color;
        final.a = 1f;
        startImage.color = final;
    }

    IEnumerator TypeEmail()
    {
        string fullText = emailText.text; // 保存完整文本
        emailText.text = "";              // 清空

        foreach (char letter in fullText)
        {
           emailText.text += letter;
           yield return new WaitForSeconds(typeSpeed);
        }
    }
}
