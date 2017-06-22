using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGem : MonoBehaviour {

    private Transform m_Transform;
    private Transform son_Transform;

	void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        son_Transform = m_Transform.FindChild("gem 3");
	}
	
	void Update () {
        son_Transform.Rotate(Vector3.up);
	}
}
