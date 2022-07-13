using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_CharacterPool : MonoBehaviour
{
    static SC_CharacterPool uniqueinstance;
    [SerializeField] public GameObject[] characterPool = new GameObject[130];
    public static SC_CharacterPool _instance
    {
        get { return uniqueinstance; }
    }

    private void Awake()
    {
        uniqueinstance = this;
        DontDestroyOnLoad(gameObject);
    }

}
