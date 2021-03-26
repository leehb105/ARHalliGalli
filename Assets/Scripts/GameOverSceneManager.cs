using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//게임이 오버 되면 창을 띄운다
public class GameOverSceneManager : MonoBehaviour
{
    public TextMeshProUGUI resultMSG;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnClickRestart()
    {
        //버튼이 눌리면 게임신으로 이동한다
        SceneManager.LoadScene("GameScene");
        
    }
    // Update is called once per frame
    void Update()
    {
        if(PlayManager.Instance.winState == 0)
        {
            //무승부
            resultMSG.text = "무 승 부";
        }else if(PlayManager.Instance.winState == 1)
        {
            //이겼음
            resultMSG.text = "승 리";
        }
        else if (PlayManager.Instance.winState == 2)
        {
            //졌음
            resultMSG.text = "패 배";
        }
    }
}
