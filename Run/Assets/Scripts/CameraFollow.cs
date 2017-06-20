using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform m_Transform;
    private Transform m_player;

    public bool startFollow = false;

    private Vector3 normalPos;  //

 	void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        normalPos = m_Transform.position;    //
        m_player = GameObject.Find("cube_books").GetComponent<Transform>();
    }
	
 	void Update () {
        CameraMove();
    }

    private void CameraMove()
    {
        if (startFollow)
        {
            Vector3 nextPos = new Vector3(m_Transform.position.x, m_player.position.y + 1.5f, m_player.position.z);
            m_Transform.position = Vector3.Lerp(m_Transform.position, nextPos, Time.deltaTime);
        }
    }

    public void ResetCamera()
    {
        m_Transform.position = normalPos;
    }
}
