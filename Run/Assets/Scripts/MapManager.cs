using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    private GameObject m_prefab_tile;
    private GameObject m_prefab_wall;
    private GameObject m_prefab_spikes;
    private GameObject m_prefab_sky_spikes;
    private GameObject m_prefab_gam;

    public List<GameObject[]> mapList = new List<GameObject[]>();

    private Transform m_Transform;

    public float bottomLength = Mathf.Sqrt(2) * 0.254f;

    private Color colorOne = new Color(124 / 255f, 155 / 255f, 230 / 255f);
    private Color colorTwo = new Color(125 / 225f, 169 / 255f, 233 / 255f);
    private Color colorWall = new Color(87 / 255f, 93 / 255f, 169 / 255f);

    int index = 0;
    private PlayerController m_PlayerController;

    //概率
    private int pr_hole = 0;
    private int pr_spikes = 0;
    private int pr_sky_spikes = 0;
    private int pr_gem = 2;

 	void Start () {
        m_prefab_tile = Resources.Load("tile_white") as GameObject;
        m_prefab_wall = Resources.Load("wall2") as GameObject;
        m_prefab_spikes = Resources.Load("moving_spikes") as GameObject;
        m_prefab_sky_spikes = Resources.Load("smashing_spikes") as GameObject;
        m_prefab_gam = Resources.Load("gem 2") as GameObject;
        m_Transform = gameObject.GetComponent<Transform>();
        m_PlayerController = GameObject.Find("cube_books").GetComponent<PlayerController>();
        CreateMapItem(0);

    }
	
 	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            string str = "";
            for(int i = 0; i < mapList.Count; i++)
            {
                for (int j = 0; j < mapList[i].Length; j++)
                {
                    mapList[i][j].name = i + "--" + j;
                    str += mapList[i][j].name;
                    str += "     ";
                }
                str += "\n";
            }
            Debug.Log(str);
        }
	}

    /// <summary>
    /// 创建地图段
    /// </summary>
    public void CreateMapItem(float offsetZ) 
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject[] item = new GameObject[6];
            for (int j = 0; j < 6; j++)
            {
                Vector3 pos = new Vector3(j * bottomLength, 0, offsetZ + i * bottomLength);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = null;
                if (j == 0 || j == 5)
                {
                    tile = GameObject.Instantiate(m_prefab_wall, pos, Quaternion.Euler(rot));
                    tile.GetComponent<MeshRenderer>().material.color = colorWall;
                }
                else
                {
                    int pr = CalcPR();
                    if (pr == 0)
                    {
                        tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot));
                        tile.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorOne;
                        tile.GetComponent<MeshRenderer>().material.color = colorOne;
                    }
                    else if (pr == 1)
                    {
                        tile = new GameObject();
                        tile.GetComponent<Transform>().position = pos;
                        tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                    }
                    else if (pr == 2) 
                    {
                        tile = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot));
                    }
                    else if (pr == 3) 
                    {
                        tile = GameObject.Instantiate(m_prefab_sky_spikes, pos, Quaternion.Euler(rot));
                    }
                }
                tile.GetComponent<Transform>().SetParent(m_Transform);
                item[j] = tile;
            }
            mapList.Add(item);

            GameObject[] item2 = new GameObject[5];
            for (int j = 0; j < 5; j++)
            {
                Vector3 pos = new Vector3(j * bottomLength + bottomLength / 2, 0, offsetZ + i * bottomLength + bottomLength / 2);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = null;
                int pr = CalcPR();
                if (pr == 0) 
                {
                    tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot));
                    tile.GetComponent<MeshRenderer>().material.color = colorTwo;
                    tile.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorTwo;
                    int gemPr = CalcGemPR();
                    if (gemPr==1)
                    {
                        GameObject gem = GameObject.Instantiate(m_prefab_gam, pos + new Vector3(0, 0.06f, 0), Quaternion.identity);
                        gem.GetComponent<Transform>().SetParent(tile.GetComponent<Transform>());
                    }
                }
                else if (pr == 1)
                {
                    tile = new GameObject();
                    tile.GetComponent<Transform>().position = pos;
                    tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                }
                else if (pr == 2)
                {
                    tile = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot));
                }
                else if (pr == 3)
                {
                    tile = GameObject.Instantiate(m_prefab_sky_spikes, pos, Quaternion.Euler(rot));
                }
                tile.GetComponent<Transform>().SetParent(m_Transform);
                item2[j] = tile;
            }
            mapList.Add(item2);
        }
    }

    public void StartTileDowm()
    {
        StartCoroutine("TileDowm");
    }

    public void StopTileDowm()
    {
        StopCoroutine("TileDowm");
    }

    private IEnumerator TileDowm()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < mapList[index].Length; i++)
            {
                Rigidbody rb = mapList[index][i].AddComponent<Rigidbody>();
                rb.angularVelocity = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * Random.Range(1f, 10f);
                GameObject.Destroy(mapList[index][i], 1.0f);
            }
            if (m_PlayerController.z == index)
            {
                StopTileDowm();
                m_PlayerController.gameObject.AddComponent<Rigidbody>();
                m_PlayerController.StartCoroutine("GameOver", true);
            }
            index++;
        }
    }

    /// <summary>
    /// 计算概率
    /// 0:瓷砖
    /// 1:洞
    /// 2:地面陷阱
    /// 3:天空陷阱
    /// </summary>
    private int CalcPR()
    {
        int pr = Random.Range(1, 100);

        if (pr <= pr_hole)
        {
            return 1;
        }
        else if (31 <pr && pr< pr_spikes + 30) 
        {
            return 2;
        }
        else if (61<pr && pr< pr_sky_spikes + 60) 
        {
            return 3;
        }

        return 0;
    }

    /// <summary>
    /// 宝石概率
    /// </summary>
    /// <returns>0生成,1不生成</returns>
    private int CalcGemPR()
    {
        int pr = Random.Range(1, 100);
        if (pr <= pr_gem) 
        {
            return 1;
        }
        return 0;
    }

    /// <summary>
    /// 增加概率
    /// </summary>
    public void AddPR()
    {
        pr_hole += 2;
        pr_spikes += 2;
        pr_sky_spikes += 2;
    }
    
    public void ResetGameMap() //
    {
        Transform[] sonTransform = m_Transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < sonTransform.Length; i++)
        {
            GameObject.Destroy(sonTransform[i].gameObject);
        }

        pr_hole = 0;
        pr_spikes = 0;
        pr_sky_spikes = 0;
        pr_gem = 2;

        index = 0;
        mapList.Clear();
        CreateMapItem(0);
    }

}
