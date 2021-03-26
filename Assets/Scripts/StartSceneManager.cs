using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public GameObject soundManger;//사운드 매니저 dondestroy
    public void OnClickStart()
    {
        print("OnClickStartBtn수행됨");
        //현재 Scene을 다시 Load하고 싶다.
        //SceneManager.LoadScene(0);//사용하고 있지 않는 Scene이 0번이다.
        SceneManager.LoadScene("GameScene");//int형과 같은 방법
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);//현재 씬에서 가져오기, 실행되면 씬 초기화 된다.
        DontDestroyOnLoad(soundManger);
        SoundManager.instance.BtnClickSound();
    }
    public void OnClickExit()
    {
        print("OnClickExit수행됨");
        SoundManager.instance.BtnClickSound();
        Application.Quit();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
