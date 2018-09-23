using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

    JsonData loadData;
    JsonData playerData;        //현재 플레이어 데이터
    public PlayerState state;
    
    public void StartButton()
    {
        int slot0=0, slot1=0, slot2=0, slot3=0, slot4=0;
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        slot0 = GameObject.Find("DSlot0").transform.GetChildCount();
        slot1 = GameObject.Find("DSlot1").transform.GetChildCount();
        slot2 = GameObject.Find("DSlot2").transform.GetChildCount();
        slot3 = GameObject.Find("DSlot3").transform.GetChildCount();
        slot4 = GameObject.Find("DSlot4").transform.GetChildCount();
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.


        if (slot0 + slot1 + slot2 + slot3 + slot4 == 5)
        {
            Save();
            Debug.Log("덱저장완료");
        }
        else
        {
            Debug.Log("덱을 다 채워 주세요");
        }


    }


    void Save()
    {
        loadData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Litjson/Player.json"));
        string Username = loadData["username"].ToString();
        bool Cat = (bool)loadData["cat"];


        string[] s = new string[5];
        s[0] = (GameObject.Find("DSlot0").transform.GetChild(0).gameObject.name);
        s[1] = (GameObject.Find("DSlot1").transform.GetChild(0).gameObject.name);
        s[2] = (GameObject.Find("DSlot2").transform.GetChild(0).gameObject.name);
        s[3] = (GameObject.Find("DSlot3").transform.GetChild(0).gameObject.name);
        s[4] = (GameObject.Find("DSlot4").transform.GetChild(0).gameObject.name);
        state = new PlayerState(Username, Cat, s);

        //state의 정보를 Json화 시켜서 playerData에 저장
        playerData = JsonMapper.ToJson(state);
        Debug.Log(playerData);

        //Player.json 안에 playerData의 모든 값을 String으로 기록해라
        File.WriteAllText(Application.dataPath + "/Litjson/Player.json", playerData.ToString());
    }
}
