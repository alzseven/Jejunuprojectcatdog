using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using LitJson;
using System.IO;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject item; //itemBeingDragged

    Vector3 startPosition;
    Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        CardLoad();
        item = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)

    {
        item = null;

        if (transform.parent == startParent || transform.parent == transform.root)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }


    void CardLoad()                             //카드정보로드&정보표시
    {
        string s = "";
        s = this.name;
        string jsonStr = File.ReadAllText(Application.dataPath + "/Resources/Litjson/Card.json");
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