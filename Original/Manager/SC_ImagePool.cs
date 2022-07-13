using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class SC_ImagePool : MonoBehaviour
{
    
    static SC_ImagePool _uniqueInstance;
    //0~9 PlayerCharacter
    //10~19 Monster
    //20~29 Item
    [SerializeField]Sprite[] m_Image = new Sprite[120];
    public int TotalImageCount;
    static public SC_ImagePool _Instance {
        get { return _uniqueInstance; } 
    }

    

    private void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(gameObject);
        TotalImageCount = m_Image.Length;
    }

    private void Start()
    {
        
    }
    public Sprite getImage(SC_PublicDefine.eUnitName UnitName)
    {
        return m_Image[(int)UnitName];
    }
}
