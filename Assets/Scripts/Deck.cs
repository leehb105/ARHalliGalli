using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//넘겨받은 카드를 소유하고 싶다
public class Deck : MonoBehaviour
{
    public List<CardInfo> list = new List<CardInfo>();
    //플레이메니저에서 던진 카드를 받아서 플레이어,상대방의덱에 쌓아준다
    public void SetCard(CardInfo card, Player player)
    {
        list.Add(card);
        //card.transform.position = Vector3.Lerp(transform.position, player.transform.position, 1.0f);
        card.transform.parent = transform;
        card.transform.localPosition = Vector3.zero;
        float i = (float)list.Count / 10;
        card.transform.localPosition = new Vector3(0, i, 0);//덱의 순서대로 쌓아주기 위해 위치 재 설정
        card.transform.localRotation = Quaternion.Euler(90, 0, 0);

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
