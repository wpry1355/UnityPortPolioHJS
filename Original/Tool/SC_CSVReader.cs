using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;


public class SC_CSVReader : MonoBehaviour
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string fileName)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = new TextAsset();

        FileInfo CheckSaveData = new FileInfo(Application.persistentDataPath + "/SaveData/" + fileName + ".csv");

        bool isSaveFile = CheckSaveData.Exists;

        string TargetText = "";
        if (isSaveFile)
        {
            //세이브 파일이 있으니 세이브 파일에서 정보를 로드
            StreamReader SR = new StreamReader(Application.persistentDataPath + "/SaveData/" + fileName + ".csv");
            TargetText = SR.ReadToEnd();
            SR.Close();

        }
        else
        {
            //세이브 파일이 없으니 기본 데이터(리소스)에서 정보를 로드
            data = Resources.Load(fileName) as TextAsset;
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData");
            TargetText = data.text;
        }
        

        var lines = Regex.Split(TargetText, LINE_SPLIT_RE);
        if (lines.Length <= 1) return list;
        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }


        return list;
    }

    public static void WriteCsv(List<Dictionary<string, object>> rowData, string filePath, int valueCount, int KeyLength)
    {
        string[][] tmp = new string[valueCount][];
        int Count = 0;
        string[] stringKey = new string[KeyLength];
        string[][] colTemp = new string[KeyLength][];

        foreach (string s in rowData[0].Keys)
        {
            stringKey[Count] = s;
            Count++;
        }

        tmp[0] = stringKey;

        for (int j = 0; j < valueCount - 1; j++)
        {
            string[] stringTmp = new string[KeyLength];
            for (int i = 0; i < KeyLength; i++)
            {
                stringTmp[i] = rowData[j][stringKey[i]].ToString();
            }
            tmp[j + 1] = stringTmp;
        }


        int length = tmp.GetLength(0);
        string delimiter = ",";
        StringBuilder sb = new StringBuilder();
        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, tmp[index]));
        }

        StreamWriter outStream = System.IO.File.CreateText(filePath);

        outStream.WriteLine(sb);
        outStream.Close();

    }


    public static int getKeyCount(List<Dictionary<string, object>> rowData)
    { 
        int Count = 0;
        foreach (string s in rowData[0].Keys)
        {
            Count++;
        }
        return Count;
    }
}
