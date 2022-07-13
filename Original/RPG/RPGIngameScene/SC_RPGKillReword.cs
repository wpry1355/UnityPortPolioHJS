using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_RPGKillReword : MonoBehaviour
{

    private void Awake()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
        Destroy(gameObject, 2f);
    }
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+ 0.01f, gameObject.transform.position.z);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b,gameObject.GetComponent<SpriteRenderer>().color.a-0.005f);
    }
}
