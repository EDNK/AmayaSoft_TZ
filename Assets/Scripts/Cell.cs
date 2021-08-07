using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;

public class Cell : MonoBehaviour
{
    [SerializeField] private UnityEvent ClickEvent; 
    [SerializeField] private ParticleSystem starEffect; 
    [SerializeField] private Transform CellData;
    [SerializeField] private string currentId;
    public string CurrentId=>currentId;
    public LevelGenerator gen {set; get;}
    [SerializeField] private SpriteRenderer sprRend;
    [SerializeField] private AnimationCurve animCurveX;
    [SerializeField] private AnimationCurve animCurveY;
    
    private Vector2 size=new Vector2(0.75f,0.75f);
    
    public bool isCorrect {set; get;}

    public void SetInfo(CardData cd)
    {
        currentId=cd.Identifier;
        sprRend.sprite=cd.Sprite;
        sprRend.size=size;
        isCorrect=false;
    }
    
    private void OnMouseDown() {
        ClickEvent.Invoke();
    }

    public void OnClick()
    {
        if (isCorrect)
            RightChoiceAnimation();
        else 
            WrongChoiceAnimation();
    }

    private void WrongChoiceAnimation()
    {
        CellData.DOLocalMoveX(1f,0.5f).SetEase(animCurveX);
        CellData.DOLocalMoveY(1f,0.5f).SetEase(animCurveY);
    }

    private void RightChoiceAnimation()
    {
        
        StartCoroutine(RightChoiceCoroutine());
    }

    IEnumerator RightChoiceCoroutine()
    {   
        starEffect.Play(true);
        CellData.DOScale(1.25f,0.25f).SetLoops(8,LoopType.Yoyo);
        yield return new WaitForSeconds(2f);
        DOTween.KillAll();
        gen.LevelEnd.Invoke();
        
    }
}
