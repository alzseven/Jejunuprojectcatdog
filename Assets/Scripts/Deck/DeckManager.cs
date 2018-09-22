using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.UI;


public class PlayerState
{
    public string username;
    public bool cat;
    public int[] decklist;


    //생성자
    public PlayerState(string username, bool cat, int[] decklist)
    {
        this.username = username;
        this.cat = cat;
        this.decklist = decklist;
    }
}
public class DeckManager : MonoBehaviour
{
    public GameObject card = null;
    public PlayerState loadState;     //세이브한 정보를 불러올 클래스 변수

    JsonData playerData;        //현재 플레이어 데이터
    JsonData loadData;          //불러와서 저장할 데이터


    public Sprite[] CatSprite;
    public Sprite[] DogSprite;

    private void Start()
    {
        loadData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Litjson/Player.json"));
        bool Cat = (bool)loadData["cat"];
        Debug.Log(Cat);

        PrefabMake(Cat);

    }

    void PrefabMake(bool a)             //카드 덱 프리팹 생성
    {
        string tribe="dog";
        Sprite[] sprite =DogSprite;
        if (a == true)                  //고양이일 경우
        {
            tribe="cat";
            sprite = CatSprite;
        }


        for (int i=0;i<=4;i++)
        {
            Transform parent1 = GameObject.Find("Slot"+i).GetComponent<Transform>();
            GameObject Temp1 = Instantiate(card, Vector3.zero, Quaternion.identity) as GameObject;          //첫째줄
            Temp1.transform.SetParent(parent1);
            Temp1.transform.localPosition = new Vector3(-710 + i * 220, 300, 0);
            Temp1.name = tribe+i;
            Temp1.GetComponent<Image>().sprite =sprite[i];

            int x = i + 5;                                                                                  //둘째줄
            Transform parent2 = GameObject.Find("Slot"+x).GetComponent<Transform>();
            GameObject Temp2 = Instantiate(card, Vector3.zero, Quaternion.identity) as GameObject;
            Temp2.transform.SetParent(parent2);
            Temp2.transform.localPosition = new Vector3(-710 + i * 220, 0, 0);
            Temp2.name = tribe + x;
            Temp2.GetComponent<Image>().sprite = sprite[x];
        }

    }



}