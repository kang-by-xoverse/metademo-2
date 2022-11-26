using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class grab : MonoBehaviour
{
    private bool hold;
    public KeyCode grabKey;
    public bool canGrab;

    IEnumerator getRequest(string uri, string color){

        // send get request
        Debug.Log(color);
        UnityWebRequest uwr = UnityWebRequest.Get(uri+"?variant="+color);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

    }

    

    void Update()
    {
        if (canGrab)
        {
            if (Input.GetKey(grabKey))
            {
                hold = true;

                if (Input.GetKeyDown(KeyCode.E))
                {

                    string color = GetComponent<FixedJoint>().connectedBody.gameObject.GetComponent<Renderer>().material.color.ToString();
                    StartCoroutine(getRequest("https://kang-api.onrender.com/api/ping", color));
                    hold = false;
                    Destroy(GetComponent<FixedJoint>().connectedBody.gameObject);
                }
            }
            else
            {
                hold = false;
                Destroy(GetComponent<FixedJoint>());
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (hold && col.transform.tag != "Player")
        {
            Rigidbody rb = col.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                FixedJoint fj = transform.gameObject.AddComponent(typeof(FixedJoint)) as FixedJoint;
                fj.connectedBody = rb;
            }
            else
            {
                FixedJoint fj = transform.gameObject.AddComponent(typeof(FixedJoint)) as FixedJoint;
            }
        }
    }
}
