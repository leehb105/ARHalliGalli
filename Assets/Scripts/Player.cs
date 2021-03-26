using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// 나의 아바타이다. 카드를 소유하고있어야한다.
public class Player : MonoBehaviour
{

    public List<CardInfo> cardList = new List<CardInfo>();
    internal bool isPlayer;

    public int GetCardListCount
    {
        get { return cardList.Count; }
    }

    //덱에 있는 모든 카드를 받아서 플레이어의 자식으로 넣어준다
    public void WinPlayerGetCards()
    {
        Deck deck = PlayManager.Instance.playerDeck;
        for (int i = 0; i < deck.list.Count; i++)
        {
            cardList.Add(deck.list[i]);
            deck.list[i].transform.parent = transform;
            deck.list[i].transform.localPosition = Vector3.zero;
            deck.list[i].transform.localRotation = Quaternion.Euler(270, 0, 0);

        }
        PlayManager.Instance.playerDeck.list = new List<CardInfo>();


        deck = PlayManager.Instance.otherDeck;
        for (int i = 0; i < deck.list.Count; i++)
        {
            cardList.Add(deck.list[i]);
            deck.list[i].transform.parent = transform;
            deck.list[i].transform.localPosition = Vector3.zero;
            deck.list[i].transform.localRotation = Quaternion.Euler(270, 0, 0);
        }
        PlayManager.Instance.otherDeck.list = new List<CardInfo>();
    }//수정요망


    public void GetCards(List<CardInfo> getList)
    {

        for (int i = 0; i < getList.Count; i++)
        {
            cardList.Add(getList[i]);
            getList[i].transform.parent = transform;
            float temp = (float)cardList.Count / 10;
            getList[i].transform.localPosition = new Vector3(0, temp, 0);//덱의 순서대로 쌓아주기 위해 위치 재 설정
            getList[i].transform.localRotation = Quaternion.Euler(270, 0, 0);
        }
        //print("+++++++++플레이어 소유 카드+++++++++");
        PrintCardList(cardList);
    }

    internal CardInfo GetCard()
    {
        CardInfo card = cardList[0];
        cardList.RemoveAt(0);
        return card;
    }

    public void PrintCardList(List<CardInfo> printList)
    {
        for (int i = 0; i < printList.Count; i++)
        {
            //print("출력: " + printList[i].cardType.ToString() + printList[i].number.ToString());
        }
        //print("총 " + printList.Count + "장");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
