using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMapmanager : MonoBehaviour {

    private GameObject m_prefab_tile;
    private GameObject m_prefab_wall;
    private Transform m_Transform;

    private Color colorOne = new Color(124 / 255f, 155 / 255f, 230 / 255f);
    private Color colorTwo = new Color(125 / 225f, 169 / 255f, 233 / 255f);
    private Color colorWall = new Color(87 / 255f, 93 / 255f, 169 / 255f);

    private float bottomLength = Mathf.Sqrt(2) * 0.254f;

    public List<GameObject[]> listMap = new List<GameObject[]>();

    private int index = 0;

    private MyPlayerConcrol m_MyMapConcrol;

    private GameObject m_prefab_spike;
    private GameObject m_prefab_sky_spike;
    private GameObject m_gem;

    private int pr_hole = 0;
    private int pr_spike = 0;
    private int pr_sky_spike = 0;
    private int pr_gem = 2;

    private int PrGem()
    {
        int pr = Random.Range(1, 100);
        if (pr < pr_gem)
        {
            return 1;
        }
            return 0;      
    }



    void Start()
    {
        m_prefab_tile = Resources.Load("tile_white") as GameObject;
        m_prefab_wall = Resources.Load("wall2") as GameObject;
        m_prefab_spike = Resources.Load("moving_spikes") as GameObject;
        m_prefab_sky_spike = Resources.Load("smashing_spikes") as GameObject;
        m_Transform = gameObject.GetComponent<Transform>();
        m_MyMapConcrol = GameObject.Find("cube_books").GetComponent<MyPlayerConcrol>();
        m_gem = Resources.Load("gem 2") as GameObject;
        CreateMapItem(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string str = "";
            for (int i = 0; i < listMap.Count; i++)
            {
                for (int j = 0; j < listMap[i].Length; j++)
                {
                    str += listMap[i][j].name;
                    listMap[i][j].name = i + "--" + j;
                }
                str += "\n";
            }
            Debug.Log(str);
        }
    }

    public void CreateMapItem(float offsetZ)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject[] item_1 = new GameObject[6];
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
                        tile.GetComponent<MeshRenderer>().material.color = colorOne;
                        tile.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorOne;

                        int pr_gem = PrGem();
                        if (pr_gem == 1) 
                        {
                            GameObject gem = GameObject.Instantiate(m_gem, pos + new Vector3(0, 0.06f, 0), Quaternion.identity);
                            gem.gameObject.GetComponent<Transform>().SetParent(tile.GetComponent<Transform>());
                        }
                    }
                    else if (pr == 1)
                    {
                        tile = new GameObject();
                        tile.GetComponent<Transform>().position = pos;
                        tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                    }
                    else if (pr ==2)
                    {
                        tile = GameObject.Instantiate(m_prefab_spike, pos, Quaternion.Euler(rot));
                    }
                    else if (pr ==3)
                    {
                        tile = GameObject.Instantiate(m_prefab_sky_spike, pos, Quaternion.Euler(rot));
                    }
                }
                tile.GetComponent<Transform>().SetParent(m_Transform);
                item_1[j] = tile;
            }
            listMap.Add(item_1);

            GameObject[] item_2 = new GameObject[5];
            for (int j = 0; j < 5; j++)
            {
                Vector3 pos = new Vector3(j * bottomLength + bottomLength / 2, 0, offsetZ + i * bottomLength + bottomLength / 2);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot));
                tile.GetComponent<Transform>().SetParent(m_Transform);
                tile.GetComponent<MeshRenderer>().material.color = colorTwo;
                tile.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorTwo;
                item_2[j] = tile;
            }
            listMap.Add(item_2);
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
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < listMap[index].Length; i++)
            {
                Rigidbody rb = listMap[index][i].AddComponent<Rigidbody>();
                rb.angularVelocity = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * Random.Range(1f, 10f);
            }
            if (index == m_MyMapConcrol.z)
            {
                m_MyMapConcrol.gameObject.AddComponent<Rigidbody>();
                m_MyMapConcrol.StartCoroutine("GameOver", true);
                StopTileDowm();
            }
            index++;
        }
    }

    /// <summary>
    /// 路障核心
    /// </summary>
    private int CalcPR()
    {
        int pr = Random.Range(1, 100);
        if (pr <= pr_hole) 
        {
            return 1;
        }
        else if (31<pr && pr < pr_spike + 30) 
        {
            return 2;
        }
        else if(61<pr && pr < pr_sky_spike + 60) 
        {
            return 3;
        }
        return 0;
    }
    
    public void AddPR()
    {
        pr_hole += 2;
        pr_spike += 2;
        pr_sky_spike += 2;
    }
}
