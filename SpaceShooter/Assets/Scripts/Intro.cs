using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{

    [SerializeField] List<GameObject> slides = new List<GameObject>();
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject nextBtn;
    [SerializeField] string nextScene;
    int currentSlide = 0;
    float fadeSpeed = 1;
    float fadeAmt = 0;

    int fadeType = 0;
    Image fadeImg;
    TextMeshProUGUI fadeText;

    bool finalFade = false;

    [SerializeField] bool isOutro = false;
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        fadeImg = slides[0].GetComponent<Image>();
        fadeImg.gameObject.SetActive(true);
        if(isOutro)
        {
            Debug.Log("destroying player");
            SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Player"), SceneManager.GetActiveScene());
            gameManager.DestroyPlayer();
        }
    }

    private void Update()
    {
        if (fadeAmt < 1)
        {
            fadeAmt += fadeSpeed * Time.deltaTime;
            if(fadeType == 0)
            {
                Color tempColor = fadeImg.color;
                tempColor.a = fadeAmt;
                fadeImg.color = tempColor;
            }
            else
            {
                Color tempColor = fadeText.color;
                tempColor.a = fadeAmt;
                fadeText.color = tempColor;
            }

        }
        else
        {
            if(finalFade)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    public void NextSlide()
    {
        if(fadeType == 0)
        {
            Color tempColor = fadeImg.color;
            tempColor.a = 1;
            fadeImg.color = tempColor;
        }
        else
        {
            Color tempColor = fadeText.color;
            tempColor.a = 1;
            fadeText.color = tempColor;
        }
        currentSlide += 1;
        if(currentSlide == slides.Count - 1)
        {
            nextBtn.SetActive(false);
            if (isOutro)
                ExitOutro();
            else
                ExitIntro();
        }
        else
        {
            fadeImg = null;
            fadeText = null;
            fadeImg = slides[currentSlide].GetComponent<Image>();
            fadeText = slides[currentSlide].GetComponent<TextMeshProUGUI>();
            if (fadeImg == null)
            {
                fadeType = 1;
                Color tempColor = fadeText.color;
                tempColor.a = 0;
                fadeText.color = tempColor;
                fadeText.gameObject.SetActive(true);
            }
            else
            {
                fadeType = 0;
                Color tempColor = fadeImg.color;
                tempColor.a = 0;
                fadeImg.color = tempColor;
                fadeImg.gameObject.SetActive(true);
            }
            fadeAmt = 0;
        }
    }

    void ExitIntro()
    {
        gameManager.SceneTransition(2, nextScene, "inPlay");
    }

    void ExitOutro()
    {
        Destroy(gameManager.gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
