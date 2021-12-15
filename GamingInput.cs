using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Text;

public class GamingInput : MonoBehaviour
{
    void Awake()
    {
#if UNITY_EDITOR
        PlayerInput inputComp = GetComponent<PlayerInput>();
        InputActionAsset IAssets = inputComp.actions;
        Dictionary<string, List<string>> actionDict = new Dictionary<string, List<string>>();

        foreach (var item in IAssets.actionMaps)
        {
            foreach(var bindItem in item.bindings)
            {
                string actionname = bindItem.action;//"Move" 
                string keyname = bindItem.path;//"<Keyboard>/w" "<Gamepad>/buttonSouth"

                if (actionDict.ContainsKey(actionname))
                {
                    actionDict[actionname].Add(keyname);
                    //Debug.Log("有重复的:" + actionname + " >>>> " + keyname +"count="+ actionDict[actionname].Count);
                }
                else
                {
                    //Debug.Log("<color=#00ff00>新增</color>:" + actionname + " >>>> " + keyname);
                    List<string> newlist = new List<string>();
                    newlist.Add(keyname);
                    actionDict.Add(actionname, newlist);
                }
            }
        }

        string keyboradKeyStr = "<Keyboard>";
        string gamepadKeyStr = "<Gamepad>";
        string actionListStr = "num,Action,Keyboard,Gamepad\n";
        int count = 0;
        string formStr = "#num#,#action#,#keybord#,#gamepad#\n";
        foreach (var dic_item in actionDict)
        {
            string keybordStr = "";
            string gamepadStr = "";
            List<string> keyList = dic_item.Value;
            foreach(string valueStr in keyList)
            {
                string[] valueStrList = valueStr.Split('/');
                if (valueStrList[0] == keyboradKeyStr)
                {
                    keybordStr += valueStrList[1]+" ";
                }
                else if (valueStrList[0] == gamepadKeyStr)
                {
                    if (valueStrList.Length >=3)
                    {
                        gamepadStr += valueStrList[1] +" -> "+ valueStrList[2];
                    }
                    else
                    {
                        gamepadStr += valueStrList[1];
                    }

                }
            }
            count++;
            string lineStr = formStr;
            lineStr = lineStr.Replace("#num#", count.ToString());
            lineStr = lineStr.Replace("#action#", dic_item.Key);
            lineStr = lineStr.Replace("#keybord#", keybordStr);
            lineStr = lineStr.Replace("#gamepad#", gamepadStr);
            actionListStr += lineStr;
        }


        string script_path = Application.dataPath + "/Config/InputLog.csv";

        if (File.Exists(script_path))
        {
            File.Delete(script_path);
        }
        FileStream file = new FileStream(script_path, FileMode.CreateNew);
        StreamWriter fileW = new StreamWriter(file, Encoding.UTF8);
        fileW.Write(actionListStr);
        fileW.Flush();
        fileW.Close();
        file.Close();
    }
 #endif
}
