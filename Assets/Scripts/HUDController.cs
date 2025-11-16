using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    public List<Image> cards;
    public Image ball;
    public bool showBall;
    public Slider chargeMeter;
    public bool UseCard(int cardIndex)
    {
        if (cards[cardIndex].rectTransform.anchoredPosition.y < -50)
        {
            ResetCardPositions();
            Vector3 curPos = cards[cardIndex].rectTransform.anchoredPosition;
            curPos.y = -50;
            cards[cardIndex].rectTransform.anchoredPosition = curPos;
            return false;
        }
        return true;
    }
    public void SetCardImage (int cardIndex, Sprite cardImage)
    {
        cards[cardIndex].enabled = true;
        cards[cardIndex].sprite = cardImage;
        Vector3 curPos = cards[cardIndex].rectTransform.anchoredPosition;
        curPos.y = -90;
        cards[cardIndex].rectTransform.anchoredPosition = curPos;
    }
    public void DeleteCardImage(int cardIndex)
    {
        cards[cardIndex].sprite = null;
        cards[cardIndex].enabled = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetCardPositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ResetCardPositions()
    {
        foreach (Image card in cards)
        {
            Vector3 curPos = card.rectTransform.anchoredPosition;
            curPos.y = -90;
            card.rectTransform.anchoredPosition = curPos;
        }
    }
}
