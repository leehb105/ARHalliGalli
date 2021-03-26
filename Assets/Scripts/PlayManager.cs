using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//각자의 턴마다 카드를 내고 싶다 
//플레이어들이 카드를 내면 각자의 카드덱으로 카드를 옮기고 싶다
public class PlayManager : MonoBehaviour
{
    public static PlayManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public TextMeshProUGUI playText;
    public TextMeshProUGUI otherText;
    public Button playCardBtn;
    //Player p;
    public Deck playerDeck;
    public Deck otherDeck;
    public Player player;
    public Player other;
    float delayTime = 1.0f;
    float curTime;
    LineRenderer lr;
    public int winState = 0;//0draw, 1win, 2lose
    public GameObject resultSuccessImg;
    public GameObject resultFailmg;
    public GameObject playerTurn;
    public GameObject otherTurn;
    enum State
    {
        None,
        PlayerTurn,
        OtherTurn,
        NoCard,

    }
    State state;
    public void OnClickCardPlay()
    {
        //버튼을 누르면 카드를 한 장 내고 싶다
        SoundManager.instance.BtnClickSound();
        PlayCard();

    }
    public void OnClickMenu()
    {
        //버튼을 누르면 메뉴로 돌아감
        SoundManager.instance.BtnClickSound();
        SoundManager.instance.audioSource.Stop();
        SceneManager.LoadScene("StartScene");


    }
    //플레이어가 카드를 낸다
    public void PlayCard()
    {
        //state = State.PlayerTurn;
        SoundManager.instance.CardSound();
        playerDeck.SetCard(player.GetCard(), player);//덱으로 한장 넣어준다
        playCardBtn.interactable = false;//제출버튼 비활성화
        state = State.OtherTurn;
        //print("플레이어의 턴으로 카드 냄");
    }
    //상대방이 카드를 낸다
    public void OtherCard()
    {
        curTime += Time.deltaTime;
        if (delayTime < curTime)
        {
            curTime = 0f;
            SoundManager.instance.CardSound();
            otherDeck.SetCard(other.GetCard(), other);//덱으로 한장 넣어주고
            playCardBtn.interactable = true;//제출버튼 활성화
            state = State.PlayerTurn;
            //print("상대방의 턴으로 카드 냄");

        }
    }

    //플레이어가 보내준 카드 리스트를 매니저의 카드 리스트로 복사한다
    internal void DivideComplete()
    {
        state = State.PlayerTurn;
    }

    // Start is called before the first frame update
    void Start()
    {
        player.isPlayer = true;
        other.isPlayer = false;

        state = State.PlayerTurn;
        /*lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;*/
        resultSuccessImg.SetActive(false);
        resultFailmg.SetActive(false);
        playerTurn.SetActive(false);
        otherTurn.SetActive(false);
    }
    public void CardCheck()
    {
        if (player.GetCardListCount == 0 && other.GetCardListCount == 1)
        {
            //무승부
            winState = 0;
            state = State.NoCard;

        }else if(other.GetCardListCount == 0){
            //이겻음
            winState = 1;
            state = State.NoCard;
        }else if (player.GetCardListCount == 0)
        {
            //졌음
            winState = 2;
            state = State.NoCard;
        }
    }
    IEnumerator WaitResultImg(bool b)
    {
        if (b == true)
        {
            resultSuccessImg.SetActive(true);
        }
        else
        {
            resultFailmg.SetActive(true);
        }
        yield return new WaitForSeconds(1.0f);
        resultSuccessImg.SetActive(false);
        resultFailmg.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        playText.text = "내 카드: " + player.cardList.Count.ToString() + " 장";
        otherText.text = "AI 카드: " + other.cardList.Count.ToString() + " 장";
        RayTouch();
        if (state != State.None)
        {
            CardCheck();
        }

        // 만약 바닥카드를 놓고 0.1~3초안에 유저가 치지 않으면 어더가 종을 쳤다고 처리하고싶다.
        // CheckWinner(other, player);  를 단 1회만 호출해야함.
        switch (state)
        {
            case State.PlayerTurn:
                playCardBtn.interactable = true;
                playerTurn.SetActive(true);
                otherTurn.SetActive(false);
                break;
            case State.OtherTurn:
                OtherCard();
                playerTurn.SetActive(false);
                otherTurn.SetActive(true);
                break;
            case State.NoCard:
                //GameManager.Instance.imageRestart.SetActive(true);
                playCardBtn.interactable = false; // 게임 종료시 restart버튼만 활성화 되고 카드 분배 버튼은 disable되야한다. 만일 누르게 되면 카드풀이 없음으로 out of Index발생
                SceneManager.LoadScene("GameOverScene");
                break;
            case State.None:
                break;
        }
    }

    private void CheckWinner(Player attacker, Player notAttacker)//수정요망
    {
        // 지금 종을 때린놈이(attacker) 조건을 검사한다.
        // 만약 조건이 맞으면 attacker가 바닥카드를 가져감.
        // 그렇지 않으면 babo가 바닥 카드를 가져간다.
        // 가져가지못한 녀석의 카드가 0이면 가져간놈이 이김
        // 플레이어가 종을 때림
        //플레이어의 덱만 있을때
        //만약 덱에 있는 카드의 종류와 숫자가 승리 조건이면 플레이어가 덱의 모든 카드를 가져가고 그렇지 않으면 상대방에게 준다
        //플레이어의 카드는 있지만 상대방이 없는 경우
        if (playerDeck.list.Count > 0 && otherDeck.list.Count == 0)
        {
            print("playerDeck.list.Count: " + playerDeck.list.Count);
            //그 카드의 숫자가 5이면 
            if (playerDeck.list[playerDeck.list.Count - 1].number + 1 == 5)
            {
                StartCoroutine(WaitResultImg(true));    
                attacker.WinPlayerGetCards();
                print(attacker + "가 카드 가져감");
                state = State.PlayerTurn;
            }
            else
            {
                StartCoroutine(WaitResultImg(false));
                notAttacker.WinPlayerGetCards();
                print(notAttacker + "가 카드 가져감");
                state = State.OtherTurn;
            }
        }
        //플레이어의 덱은 비었지만 상대방의 덱만 있을 때
        else if (otherDeck.list.Count > 0 && playerDeck.list.Count == 0)
        {
            print("otherDeck.list.Count: " + otherDeck.list.Count);
            //그 카드의 숫자가 5일때
            if (otherDeck.list[otherDeck.list.Count - 1].number + 1 == 5)
            {
                StartCoroutine(WaitResultImg(true));
                attacker.WinPlayerGetCards();
                print(attacker + "가 카드 가져감");
                state = State.PlayerTurn;
            }
            else
            {
                StartCoroutine(WaitResultImg(false));
                notAttacker.WinPlayerGetCards();
                print(notAttacker + "가 카드 가져감");
                state = State.OtherTurn;
            }
        }
        //두 덱에 카드가 존재하면
        else if (otherDeck.list.Count > 0 && playerDeck.list.Count > 0)
        {
            print("playerDeck.list.Count : " + playerDeck.list.Count);
            int playerDeckNum = (playerDeck.list[playerDeck.list.Count - 1].number) + 1;
            print("playerDeckNum" + playerDeckNum);
            int otherDeckNum = (otherDeck.list[otherDeck.list.Count - 1].number) + 1;
            print("otherDeckNum" + otherDeckNum);
            //두 카드의 종류가 같고 그 합이 5일때
            if ((playerDeck.list[playerDeck.list.Count - 1].cardType == otherDeck.list[otherDeck.list.Count - 1].cardType && (playerDeckNum + otherDeckNum) == 5))
            {
                //AIRingBell();
                StartCoroutine(WaitResultImg(true));
                attacker.WinPlayerGetCards();
                print(attacker + "가 카드 가져감");
                state = State.PlayerTurn;

            }//두 카드의 종류가 같지 않고 플레이어것이 5일때 또는 두 카드의 종류가 같지 않고 상대방의 것이 5일때
            else if((playerDeck.list[playerDeck.list.Count - 1].cardType != otherDeck.list[otherDeck.list.Count - 1].cardType && playerDeckNum == 5) || 
                    (playerDeck.list[playerDeck.list.Count - 1].cardType != otherDeck.list[otherDeck.list.Count - 1].cardType && otherDeckNum == 5))
            {
                 StartCoroutine(WaitResultImg(true));
                 attacker.WinPlayerGetCards();
                 print(attacker + "가 카드 가져감");
                 state = State.PlayerTurn;
            }
            else
            {
                StartCoroutine(WaitResultImg(false));
                notAttacker.WinPlayerGetCards();
                print(notAttacker + "가 카드 가져감");
                state = State.OtherTurn;
            }
        }
        else
        {
            StartCoroutine(WaitResultImg(false));

            notAttacker.WinPlayerGetCards();
            print(notAttacker + "가 카드 가져감");
            state = State.OtherTurn;
        }
    }
    public void AIRingBell()
    {
        float rnd = UnityEngine.Random.Range(0.3f, 1.5f);
        rnd += Time.deltaTime;
        print("rnd" + rnd);
        if (rnd < curTime)
        {
            CheckWinner(other, player);
        }
    }
    
    private void RayTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 시선을 이용해서 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //lr.SetPosition(0, ray.origin);
            // 닿은곳의 정보를 알고싶다.
            RaycastHit hitInfo;
            // 바라보고싶다.
            if (Physics.Raycast(ray, out hitInfo))
            {
                //lr.SetPosition(1, hitInfo.point);
                if (hitInfo.transform.gameObject.tag == "Bell")//벨을 누르면
                {
                    //print("벨에 닿았음");
                    CheckWinner(player, other);
                    SoundManager.instance.RingBellSound();
                }
            }
            else
            {
                //lr.SetPosition(1, ray.origin + ray.direction * 1000);
            }
        }
    }
}
