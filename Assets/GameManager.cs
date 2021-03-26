using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//카드의 종류와 넘버를 세팅하여 카드를 생성해 준다.
//카드를 셔플하고 싶다
//카드를 사용자, 상대방에게 절반씩 나누어 준다
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public GameObject cardInfoFactory;
    public Player user;
    public Player other;
    public List<CardInfo> list; //딜러의 리스트
    /*List<CardInfo> PlayerDividedCards;
    List<CardInfo> OtherDividedCards;*/
    
    public CardInfo MakeCard(CardInfo.CardType type, int num)
    {
        GameObject obj = Instantiate(cardInfoFactory);
        obj.transform.parent = transform;
        CardInfo cardInfo = obj.GetComponent<CardInfo>();
        cardInfo.SetCardInfo(type, num);
        return cardInfo;
    }

    public void OnClickRestart()
    {//콜백함수, 이벤트함수 - OnClick이 붙는다
        print("OnClickRestart수행됨");
        //현재 Scene을 다시 Load하고 싶다.
        //SceneManager.LoadScene("SampleScene");//int형과 같은 방법
        SoundManager.instance.BtnClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//현재 씬에서 가져오기, 실행되면 씬 초기화 된다.
    }
    // Start is called before the first frame update
    void Start()
    {
        list = new List<CardInfo>();
        for (int i = 0; i < 5; i++)
        {
            //두 세트씩 생성해 준다
            list.Add(MakeCard(CardInfo.CardType.Strawberry, i));
            list.Add(MakeCard(CardInfo.CardType.Lemon, i));
            list.Add(MakeCard(CardInfo.CardType.Grape, i));
            list.Add(MakeCard(CardInfo.CardType.Banana, i));
            list.Add(MakeCard(CardInfo.CardType.Strawberry, i));
            list.Add(MakeCard(CardInfo.CardType.Lemon, i));
            list.Add(MakeCard(CardInfo.CardType.Grape, i));
            list.Add(MakeCard(CardInfo.CardType.Banana, i));
        }
        
        //PrintCardList(list);
        CardShuffle();
        CardDivide();

        print("+++++++++딜러 소유 카드+++++++++");
        PrintCardList(list);
        
    }
    public void CardShuffle()
    {
        //카드를 셔플한다
        CardInfo temp = null;
        for (int i = 0; i < list.Count - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, list.Count);
            temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
        
    }
    //입력받은 리스트의 카드타입과 번호, 총 장수를 출력해준다
    public void PrintCardList(List<CardInfo> printList)
    {
        for(int i = 0; i< printList.Count; i++)
        {
            print("출력: " + printList[i].cardType.ToString() + printList[i].number.ToString());
        }

        print("총 " + printList.Count + "장");
    }

    //카드를 사용자, 상대방에게 절반씩 나누어 준다
    public void CardDivide()
    {
        int count = list.Count;
        user.GetCards(list.GetRange(0, count / 2));//사용자에게 카드의 반을 배분
        

        //list의 절반을 삭제함
        for (int i = 0; i < count/2; i++)
        {
            list.RemoveAt(0);
        }
        //남은 절반의 카드를 상대방에게 배분
        other.GetCards(list);
        
        list.Clear();

        PlayManager.Instance.DivideComplete();
    }    

}
