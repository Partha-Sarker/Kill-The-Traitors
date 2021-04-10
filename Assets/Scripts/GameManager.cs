using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public CanvasGroup gameCanvas;
    public GameObject deadPanel, winPanel;

    public int deadEnemy = 0, allEnemy = 4, deadDelay = 4, winDelay = 4;

    public AudioSource deadAudio, winAudio;
    private void Start()
    {
        Cursor.visible = false;
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadWinPanel()
    {
        Cursor.visible = true;
        winAudio.Play();
        Invoke("WaitForWinPanel", winDelay);
    }

    public void WaitForWinPanel()
    {
        deadPanel.SetActive(false);
        winPanel.SetActive(true);
        gameCanvas.DOFade(1, 1);
    }

    public void LoadDeadPanel()
    {
        Cursor.visible = true;
        deadAudio.Play();
        Invoke("WaitForDeadPanel", deadDelay);
    }

    public void WaitForDeadPanel()
    {
        deadPanel.SetActive(true);
        winPanel.SetActive(false);
        gameCanvas.DOFade(1, 1);
    }
    public void DieEnemy()
    {
        deadEnemy++;
        if (deadEnemy == allEnemy)
            LoadWinPanel();
    }
}
