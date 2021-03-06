﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySpikes : MonoBehaviour {

    private Transform m_Transform;
    private Transform son_Transform;

    private Vector3 normalPos;
    private Vector3 targetPos;

    void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        son_Transform = m_Transform.FindChild("smashing_spikes_b").GetComponent<Transform>();
        normalPos = son_Transform.position;
        targetPos = son_Transform.position + new Vector3(0, 0.6f, 0);
        StartCoroutine("UpAndDowm");
    }
	
    private IEnumerator UpAndDowm()
    {
        while (true)
        {
            StopCoroutine("Dowm");
            StartCoroutine("Up");
            yield return new WaitForSeconds(2.0f);
            StopCoroutine("Up");
            StartCoroutine("Dowm");
            yield return new WaitForSeconds(2.0f);
        }
    }

    private IEnumerator Up()
    {
        while (true)
        {
            son_Transform.position = Vector3.Lerp(son_Transform.position, targetPos, Time.deltaTime * 25);
            yield return null;
        }
    }

    private IEnumerator Dowm()
    {
        while (true)
        {
            son_Transform.position = Vector3.Lerp(son_Transform.position, normalPos, Time.deltaTime * 25);
            yield return null;
        }
    }
}
