using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Animator : MonoBehaviour
{
    [SerializeField] private LevelGenerator gen;
    [SerializeField] private Image restartScreen;
    private Text taskText;
    
    // Анимация при старте (Bounce ячейки и Fade текст)
    public void FirstAnimation()
    {
        Transform tmp;
        var objsToAnimate=gen.Cells;
        foreach (var obj in objsToAnimate)
        {
            tmp=obj.GetComponent<Transform>();
            tmp.DOScale(4f, 0.5f).From(0f);
            tmp.GetComponent<Transform>().DOScale(2f, 0.5f).SetDelay(0.5f);
        }
        
        taskText=gen.TaskText;
        taskText.DOFade(1f,1f).From(0f).SetDelay(1f);
    }

    // Затемнение экрана(FadeIn) и кнопка рестарта
    public void GameEndsAnimation()
    {   
        restartScreen.GetComponent<CanvasGroup>().DOFade(1f,1f).From(0f);
    }

    // FadeOut от предыдущего
    public void NewGameAnimation()
    {
        restartScreen.GetComponent<CanvasGroup>().DOFade(0f,1f).From(1f);
    }

}
