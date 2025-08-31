using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEditor.PackageManager.UI;
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Slider hpBar;
    public GameObject optionViewer;
    public Image hitEffect;
    public Image gameOver;
    public bool Pause { get => optionViewer.activeSelf; }
    public PlayerHealth player;
    private Coroutine coroutine;
    public event Action<bool> OnPause;

    private void Start()
    {
        gameOver.gameObject.SetActive(false);
        player.OnDeath += () => StartCoroutine(CoGameOver());
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"score : {score}";
    }

    public void UpdateHpSlide(float hp , float maxHp)
    {
        hpBar.value = hp / maxHp;
    }

    public void OpenOption()
    {   
        optionViewer.SetActive(!optionViewer.activeSelf);
        OnPause?.Invoke(Pause);
    }
   

    public void HitEffect()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(CoHitEffect());
    }

    IEnumerator CoHitEffect()
    {
        hitEffect.color = new Color(1f, 0f, 0f, 30f / 255f);

        for(float i = 30f / 255f; i > 0f; i -= Time.deltaTime)
        {
            hitEffect.color = new Color(1f, 0f, 0f, i);
            yield return null;
        }

        hitEffect.color = new Color(1f, 0f, 0f, 0f);
        coroutine = null;
    }

    IEnumerator CoGameOver()
    {
        gameOver.gameObject.SetActive(true);
        for (float i = 0f; i <= 1f; i += 0.02f)
        {
            gameOver.color = new Color(25f / 255f, 0f, 38f / 255f, i);
            yield return null;
        }


        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


