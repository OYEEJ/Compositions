using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerConcrol : MonoBehaviour {

    private Transform m_Transfrom;
    private MyMapmanager m_MyMapManager;
    private MyCameraFollow m_MyCameraFollow;
    private MyUIManager m_MyUIManager;

    public int z = 3;
    private int x = 2;

    private Color colorOne = new Color(122 / 255f, 85 / 255f, 179 / 255f);
    private Color colorTwo = new Color(126 / 255f, 93 / 255f, 183 / 255f);

    private bool life = true;

    private int gemsource = 0;
    private int addsource = 0;

    private void Gemsource()
    {
        gemsource++;
        Debug.Log("金币：" + gemsource);
        m_MyUIManager.SaveDatedate(addsource, gemsource);
    }
    private void AddSource()
    {
        addsource++;
        Debug.Log("分数：" + addsource);
        m_MyUIManager.SaveDatedate(addsource, gemsource);
    }
    private void SaveDate()
    {
        PlayerPrefs.SetInt("m_gem", gemsource);
        if (addsource > PlayerPrefs.GetInt("m_source",0))
        {
            PlayerPrefs.SetInt("m_source", addsource);
        }
    }
    void Start()
    {
        gemsource = PlayerPrefs.GetInt("m_gem", 0);
        m_Transfrom = gameObject.GetComponent<Transform>();
        m_MyMapManager = GameObject.Find("MapManager").GetComponent<MyMapmanager>();
        m_MyCameraFollow = GameObject.Find("Main Camera").GetComponent<MyCameraFollow>();
        m_MyUIManager = GameObject.Find("UI Root").GetComponent<MyUIManager>();
    }

    public void StartGame()
    {
        SetPalyerPos();
        m_MyCameraFollow.startFollow = true;
        m_MyMapManager.StartTileDowm();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartGame();
        }
        if (life)
        {
            PlayerConteol();
        }
    }

    private void PlayerConteol()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (x != 0)
            {
                z++;
                AddSource();
            }
            if (z % 2 == 1 && x != 0)
            {
                x--;
            }
            SetPalyerPos();
            CalcPosition();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (x != 4 || z % 2 == 0)
            {
                z++;
            }
            if (z % 2 == 0 && x != 4)
            {
                x++;
                AddSource();
            }
            SetPalyerPos();
            CalcPosition();
        }

    }


    void SetPalyerPos()
    {
        Transform player = m_MyMapManager.listMap[z][x].GetComponent<Transform>();
        MeshRenderer normal_a2 = null;

        m_Transfrom.position = player.position + new Vector3(0, 0.254f / 2, 0);
        m_Transfrom.rotation = player.rotation;


        if (player.tag == "Tile") 
        {
            normal_a2 = player.FindChild("normal_a2").GetComponent<MeshRenderer>();
        }
        else if (player.tag=="Spikes")
        {
            normal_a2 = player.FindChild("moving_spikes_b").GetComponent<MeshRenderer>();
        }
        else if (player.tag == "Sky_Spikes") 
        {
            normal_a2 = player.FindChild("smashing_spikes_a2").GetComponent<MeshRenderer>();
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
        else if (normal_a2 == null)
        {
            gameObject.AddComponent<Rigidbody>();
            StartCoroutine("GameOver", true);
        }
    }

    private void CalcPosition()
    {
        if (m_MyMapManager.listMap.Count - z <= 5) 
        {
            m_MyMapManager.AddPR();
            //Debug.Log("222");
            float offsetZ = m_MyMapManager.listMap[m_MyMapManager.listMap.Count - 1][0].GetComponent<Transform>().position.z + Mathf.Sqrt(2) * 0.254f / 2;
            m_MyMapManager.CreateMapItem(offsetZ    );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Spikes_Attack")
        {
            StartCoroutine("GameOver",false);
        }
        if (other.tag == "Gem") 
        {
            GameObject.Destroy(other.gameObject.GetComponent<Transform>().parent.gameObject);
            Gemsource();
        }
    }

    private IEnumerator GameOver(bool b)
    {
        if (b)
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (life)
        {
            Debug.Log("MyGameOver");
            life = false;
            SaveDate();
        }
        //Time.timeScale = 0;  // 该方法易导致Unity退出;
    }
}
