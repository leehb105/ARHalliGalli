using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public Texture[] texture;
    //카드종류 선언
    public enum CardType
    {
        //딸기 레몬 포도 바나나
        Strawberry, // 0~4 이미지 배열 인덱스
        Lemon,      // 5~9
        Grape,      // 10~14
        Banana      // 15~19
    }
    public CardType cardType;
    public int number;

    //카드 종류, 숫자, 해당 텍스쳐 적용
    public void SetCardInfo(CardType type, int num)
    {
        cardType = type;
        int nType = (int)type * 5; // 5 는 한개의 타입이 가지는 이미지 갯수
        number = num;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material.mainTexture = texture[nType + num];
    }

   
}
