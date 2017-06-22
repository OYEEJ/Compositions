using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private GameObject m_StartUI;
    private GameObject m_GamUI;

    private UILabel m_ScoreLabel;
    private UILabel m_GemLabel;
    private UILabel m_Game_Score_Label;
    private UILabel m_Game_Gem_Label;

    private GameObject m_Left;
    private GameObject m_Right;

    private PlayerController m_PlayerControler;
    private GameObject m_PlayButton;

	void Start () {
        m_StartUI = GameObject.Find("Start_UI");
        m_GamUI = GameObject.Find("Game_UI");
        m_PlayButton = GameObject.Find("play_btn");

        m_Left = GameObject.Find("Left");
        UIEventListener.Get(m_Left).onClick = Left;
        m_Right = GameObject.Find("Right");
        UIEventListener.Get(m_Right).onClick = Right;

        m_Game_Score_Label = GameObject.Find("Game_Score_Label").GetComponent<UILabel>();
        m_Game_Gem_Label = GameObject.Find("Game_Gem_Label").GetComponent<UILabel>();
        m_ScoreLabel = GameObject.Find("Score_Label").GetComponent<UILabel>();
        m_GemLabel = GameObject.Find("Gem_Label").GetComponent<UILabel>();

        m_PlayerControler = GameObject.Find("cube_books").GetComponent<PlayerController>();

        UIEventListener.Get(m_PlayButton).onClick = PlayButtonClick;

        Init();
        m_GamUI.SetActive(false);
    }
	
    private void Init()
    {
        m_ScoreLabel.text = PlayerPrefs.GetInt("score", 0) + "";
        m_GemLabel.text = PlayerPrefs.GetInt("gem", 0) + "/100";
        m_Game_Score_Label.text = "0";
        m_Game_Gem_Label.text = PlayerPrefs.GetInt("gem", 0) + "/100";
    }

    public void UpdateDate(int score,int gem)
    {
        m_GemLabel.text = gem + "/100";                      //
        m_Game_Score_Label.text = score.ToString();
        m_Game_Gem_Label.text = gem + "/100";
    }

    private void PlayButtonClick(GameObject go)
    {
        Debug.Log("Game Start");
        m_StartUI.SetActive(false);
        m_GamUI.SetActive(true);
        m_PlayerControler.StartGame();
    } 

    public void ResetUI()                                     //
    {
        m_StartUI.SetActive(true);
        m_GamUI.SetActive(false);
        m_Game_Score_Label.text = "0";
    }

    private void Left(GameObject go)
    {
        m_PlayerControler.Left();
    }
    private void Right(GameObject go)
    {
        m_PlayerControler.Right();
    }
}
