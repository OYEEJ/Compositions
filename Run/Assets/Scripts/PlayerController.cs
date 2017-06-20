using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Transform m_Transform;
    private MapManager m_MapManager;
    private CameraFollow m_CameraFollow;
    private UIManager m_UIManager;

    private Color colorOne = new Color(122 / 255f, 85 / 255f, 179 / 255f);
    private Color colorTwo = new Color(126 / 255f, 93 / 255f, 183 / 255f);

    public int z = 3;
    private int x = 2;

    private bool life = true;
    private int gemCount = 0 ;
    private int scoreCount = 0;

    private void AddGemCount()
    {
        gemCount++;
        Debug.Log("宝石数"+gemCount);
        m_UIManager.UpdateDate(scoreCount,gemCount);
    }
    private void AddScoreCount()
    {
        scoreCount++;
        Debug.Log("分数" + scoreCount);
        m_UIManager.UpdateDate(scoreCount, gemCount);
    }

    private void SaveDate()
    {
        PlayerPrefs.SetInt("gem", gemCount);
        if (scoreCount > PlayerPrefs.GetInt("score", 0))
        {
            PlayerPrefs.SetInt("score", scoreCount);
        }
    }

 	void Start () {
        gemCount = PlayerPrefs.GetInt("gem", 0);         //从注册表中取gem键所对应的值（regedit -- HKEY_CURRENT_USER -- SOFTWARE -- 目标）
        m_Transform = gameObject.GetComponent<Transform>();
        m_MapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        m_CameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        m_UIManager = GameObject.Find("UI Root").GetComponent<UIManager>();
    }
	
    public void StartGame()
    {
        SetPlayerPos();
        m_CameraFollow.startFollow = true;
        m_MapManager.StartTileDowm();
    }

 	void Update () {

        if (Input.GetKeyDown(KeyCode.M))
        {
            StartGame();
        }
        if (life)
        {
            PlayerConteol();
        }
    }
    
    public void Left()
    {
        if (x != 0)
        {
            z++;
            AddScoreCount();
        }

        if (z % 2 == 1 && x != 0)
        {
            x--;
        }
        SetPlayerPos();
        CalcPosition();
    }

    public void Right()
    {
        if (x != 4 || z % 2 != 1)
        {
            z++;
            AddScoreCount();
        }

        if (z % 2 == 0 && x != 4)
        {
            x++;
        }
        SetPlayerPos();
        CalcPosition();
    }

    private void PlayerConteol()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Left();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Right();
        }
    }

    private void SetPlayerPos()
    {
        Transform playerPos = m_MapManager.mapList[z][x].GetComponent<Transform>();
        MeshRenderer normal_a2 = null;

        m_Transform.position = playerPos.position + new Vector3(0, 0.254f / 2, 0);
        m_Transform.rotation = playerPos.rotation;


        if (playerPos.tag == "Tile") 
        {
            normal_a2 = playerPos.FindChild("normal_a2").GetComponent<MeshRenderer>();
        }
        else if (playerPos.tag == "Spikes") 
        {
            normal_a2 = playerPos.FindChild("moving_spikes_a2").GetComponent<MeshRenderer>();
        }
        else if (playerPos.tag == "Sky_Spikes") 
        {
            normal_a2 = playerPos.FindChild("smashing_spikes_a2").GetComponent<MeshRenderer>();
        }

        if (normal_a2 != null) 
        {
            if (z % 2 == 0)
            {
                normal_a2.material.color = colorOne;
            }
            else
            {
                normal_a2.material.color = colorTwo;
            }
        }
        else
        {
            gameObject.AddComponent<Rigidbody>();
            StartCoroutine("GameOver", true);
        }

    }

    /// <summary>
    /// 新地图
    /// </summary>
    private void CalcPosition()
    {
        if (m_MapManager.mapList.Count - z <=12)
        {
            m_MapManager.AddPR();
            Debug.Log("新地图");
            float offsetZ = m_MapManager.mapList[m_MapManager.mapList.Count - 1][0].GetComponent<Transform>().position.z + m_MapManager.bottomLength / 2;
            m_MapManager.CreateMapItem(offsetZ);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Spikes_Attack")
        {
            StartCoroutine("GameOver", false);
        }

        if (other.tag=="Gem")
        {
            GameObject.Destroy(other.gameObject.GetComponent<Transform>().parent.gameObject);
            AddGemCount();
        }
        //Debug.Log(other.gameObject.name);
    }

    private IEnumerator GameOver(bool b)
    {
        if (b)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (life)
        {
            Debug.Log("游戏结束");
            m_CameraFollow.startFollow = false;
            life = false;
            SaveDate();
            StartCoroutine("ResetGame");                  //
        }
        //Time.timeScale = 0;
    }

    private void ResetPlayer()                              //
    {
        GameObject.Destroy(gameObject.GetComponent<Rigidbody>());
        z = 3;
        x = 2;
        life = true;
        scoreCount = 0;
    }

    private IEnumerator ResetGame()                       //
    {
        yield return new WaitForSeconds(2);
        m_UIManager.ResetUI();
        m_MapManager.ResetGameMap();
        m_CameraFollow.ResetCamera();
        ResetPlayer();
    } 
}
