using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySpikes : MonoBehaviour {

    private Transform m_Transform;
    private Transform son_Tranform;

    private Vector3 normalPos;
    private Vector3 targetPos;

    void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        son_Tranform = m_Transform.FindChild("moving_spikes_b").GetComponent<Transform>();

        normalPos = m_Transform.position;
        targetPos = m_Transform.position + new Vector3(0, 0.15f, 0);

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
            son_Tranform.position = Vector3.Lerp(son_Tranform.position, targetPos, Time.deltaTime * 25);
            yield return null;
        }
    }

    private IEnumerator Dowm()
    {
        while (true)
        {
            son_Tranform.position = Vector3.Lerp(son_Tranform.position, normalPos, Time.deltaTime * 25);
            yield return null;
        }
    }
}
