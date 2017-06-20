using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUIManager : MonoBehaviour {

    private GameObject m_StartUI;
    private GameObject m_GameUI;
    private GameObject m_PlayButton;

    private UILabel m_StartUI_Source_Label;
    private UILabel m_Start_Gem_Label;
    private UILabel m_Game_Source_Label;
    private UILabel m_Game_Gem_Label;

    private MyPlayerConcrol m_MyPlayConcrol;

    void Start () {

        m_StartUI = GameObject.Find("Start_UI");
        m_GameUI = GameObject.Find("Game_UI");
        m_PlayButton = GameObject.Find("play_btn");
        m_StartUI_Source_Label = GameObject.Find("Score_Label").GetComponent<UILabel>();
        m_Start_Gem_Label = GameObject.Find("Gem_Label").GetComponent<UILabel>();
        m_Game_Source_Label = GameObject.Find("Game_Score_Label").GetComponent<UILabel>();
        m_Game_Gem_Label = GameObject.Find("Game_Gem_Label").GetComponent<UILabel>();
        m_MyPlayConcrol = GameObject.Find("cube_books").GetComponent<MyPlayerConcrol>();

        m_GameUI.SetActive(false);

        Init();

        UIEventListener.Get(m_PlayButton).onClick = PlayButtonClick;
    }

    private void Init()
    {
        m_StartUI_Source_Label.text = PlayerPrefs.GetInt("m_source", 0) + "";
        m_Start_Gem_Label.text = PlayerPrefs.GetInt("m_gem", 0) + "/100";
        m_Game_Source_Label.text = "0";
        m_Game_Gem_Label.text = PlayerPrefs.GetInt("m_gem", 0) + "/100";
    }

    public void SaveDatedate(int score,int gem)
    {
        m_Game_Source_Label.text = score.ToString();
        m_Game_Gem_Label.text = gem + "/100";
    }

    private void PlayButtonClick(GameObject go)
    {
        Debug.Log("Game Start");
        m_GameUI.SetActive(true);
        m_StartUI.SetActive(false);
        m_MyPlayConcrol.StartGame();
    }

}
