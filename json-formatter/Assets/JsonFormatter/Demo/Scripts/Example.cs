using UnityEngine;
using System;
using System.IO;
using HC.Common;


public class Example : MonoBehaviour
{
    #region イベントメソッド

    private void Start()
    {
        string json = ReadText("JsonFormatter/Demo/example.json");

        json = JsonFormatter.ToMinifyPrint(json);
        Debug.Log("MinifyPrint: " + Environment.NewLine + json);

        json = JsonFormatter.ToPrettyPrint(json, JsonFormatter.IndentType.Space);
        Debug.Log("PrettyPrint: " + Environment.NewLine + json);
    }

    #endregion


    #region メソッド

    /// <summary>
    /// テキストをファイルから読み込む
    /// </summary>
    /// <param name="path">Application.dataPath以降のファイルパス</param>
    /// <returns>読み込んだテキスト</returns>
    public static string ReadText(string path)
    {
        string text = string.Empty;

        try
        {
            using (var streamReader = new StreamReader(Path.Combine(Application.dataPath, path)))
            {
                text = streamReader.ReadToEnd();
                streamReader.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogAssertion(e.Message);
        }

        return text;
    }

    #endregion
}