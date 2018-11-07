using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class CardState
{
    public string number;
    public string cardname;
    public int hp;
    public int attack;
    public int range;
    public int attackspe;
    public int speed;
    public int delay;
    public int cost;

    //생성자
    public CardState(string number, string cardname, int hp, int attack, int range, int attackspe, int speed, int delay, int cost)
    {
        this.number = number;
        this.cardname = cardname;
        this.hp = hp;
        this.attack = attack;
        this.range = range;
        this.attackspe = attackspe;
        this.speed = speed;
        this.delay = delay;
        this.cost = cost;
    }
}

public class Click : MonoBehaviour {
    
    public void CardClick()
    {
        Load();
    }

    public void Load()                                  //카드정보로드&정보표시
    {
        string s = "";
        s = this.name;
        string jsonStr = File.ReadAllText(Application.dataPath + "Resources/Litjson/Card.json");
        JsonData playerData = JsonMapper.ToObject(jsonStr);
        for (int i = 0; i < playerData.Count; i++)
        {
            if (s == playerData[i]["number"].ToString())
            {
                GameObject.Find("cardname").GetComponent<Text>().text = playerData[i]["cardname"].ToString();
                GameObject.Find("hp").GetComponent<Text>().text = playerData[i]["hp"].ToString();
                GameObject.Find("attack").GetComponent<Text>().text = playerData[i]["attack"].ToString();
                GameObject.Find("range").GetComponent<Text>().text = playerData[i]["range"].ToString();
                GameObject.Find("attackspe").GetComponent<Text>().text = playerData[i]["attackspe"].ToString();
                GameObject.Find("speed").GetComponent<Text>().text = playerData[i]["speed"].ToString();
                GameObject.Find("delay").GetComponent<Text>().text = playerData[i]["delay"].ToString();
                GameObject.Find("cost").GetComponent<Text>().text = playerData[i]["cost"].ToString();
            }
        }
    }

}