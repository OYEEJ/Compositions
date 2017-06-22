using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraFollow : MonoBehaviour {

    private Transform m_Transform;
    private Transform m_player;

    public bool startFollow = false;

	void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        m_player = GameObject.Find("cube_books").GetComponent<Transform>();
	}
	
	void Update () {
        CameraMove();
    }

    private void CameraMove()
    {
        if (startFollow)
        {
            Vector3 pos = new Vector3(m_Transform.position.x, m_player.position.y + 1.08f, m_player.position.z);
            m_Transform.position = Vector3.Lerp(m_Transform.position, pos, Time.deltaTime);
        }
    }
}
