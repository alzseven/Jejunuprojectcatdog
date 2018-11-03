using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyDeck : MonoBehaviour
    {
        public GameObject card = null;

        public RectTransform catdogPanel;
        public RectTransform deckPanel;
        public RectTransform lobbyPanel;

        public GameObject Cat, Dog;
        private GameObject[] allcard=new GameObject[10];

        //저장해야할 변수들
        static public string[] decklist=new string[5];
        static bool cat = true;

        protected RectTransform currentPanel;

        private void Awake()
        {
            currentPanel = catdogPanel;
        }

        void Update()
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f))
            {
                if (hit.transform.name == "CatBackground")//고양이진영선택
                {
                    AnimineControl.CatAnime.SetBool("Dance", true);
                    AnimineControl.DogAnime.SetBool("Dance", false);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("고양이눌림");
                        cat = true;
                        deckPanel.gameObject.SetActive(true);
                        catdogPanel.gameObject.SetActive(false);
                        currentPanel = deckPanel;
                        PrefabMake(cat);
                    }
                }
                else if (hit.transform.name == "DogBackground")//강아지진영선택
                {
                    AnimineControl.CatAnime.SetBool("Dance", false);
                    AnimineControl.DogAnime.SetBool("Dance", true);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("강아지눌림");
                        cat = false;
                        deckPanel.gameObject.SetActive(true);
                        catdogPanel.gameObject.SetActive(false);
                        currentPanel = deckPanel;
                        PrefabMake(cat);
                    }
                }
                else
                {
                    AnimineControl.CatAnime.SetBool("Dance", false);
                    AnimineControl.DogAnime.SetBool("Dance", false);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("둘다 안눌림");
                    }
                }

            }
        }


        
        




        public Sprite[] CatSprite;
        public Sprite[] DogSprite;

        void PrefabMake(bool a)             //카드 덱 프리팹 생성
        {
            string tribe = "dog";
            Sprite[] sprite = DogSprite;
            if (a == true)                  //고양이일 경우
            {
                tribe = "cat";
                sprite = CatSprite;
            }

            for (int i = 0; i <= 4; i++)
            {
                Transform parent1 = GameObject.Find("Slot" + i).GetComponent<Transform>();
                GameObject Temp1 = Instantiate(card, Vector3.zero, Quaternion.identity) as GameObject;          //첫째줄
                Temp1.transform.SetParent(parent1);
                Temp1.transform.localScale = new Vector3(1.5f, 2.4f, 0);
                Temp1.transform.localPosition = new Vector3(-710 + i * 220, 300, 0);
                Temp1.name = tribe + i;
                Temp1.GetComponent<Image>().sprite = sprite[i];
                allcard[i]=Temp1;

                int x = i + 5;                                                                                  //둘째줄
                Transform parent2 = GameObject.Find("Slot" + x).GetComponent<Transform>();
                GameObject Temp2 = Instantiate(card, Vector3.zero, Quaternion.identity) as GameObject;
                Temp2.transform.SetParent(parent2);
                Temp2.transform.localScale = new Vector3(1.5f,2.4f, 0);
                Temp2.transform.localPosition = new Vector3(-710 + i * 220, 0, 0);
                Temp2.name = tribe + x;
                Temp2.GetComponent<Image>().sprite = sprite[x];
                allcard[x]=Temp2;

            }
        }

        
        public void StartButton()               //DECK 패널에 있는 PLAY버튼을 눌렀을 경우
        {
            int slot0 = 0, slot1 = 0, slot2 = 0, slot3 = 0, slot4 = 0;
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
                deckPanel.gameObject.SetActive(false);
                lobbyPanel.gameObject.SetActive(true);
                GameObject[] tempobj = GameObject.FindGameObjectsWithTag("PlayerInfo");
                tempobj[0].GetComponent<LobbyPlayer>().ToggleJoinButton(true,true);
                tempobj[1].GetComponent<LobbyPlayer>().ToggleJoinButton(true,true);

            }
            else
            {
                Debug.Log("덱을 다 채워 주세요");
            }
        }

        void Save()
        {
            decklist[0] = (GameObject.Find("DSlot0").transform.GetChild(0).gameObject.name);
            decklist[1] = (GameObject.Find("DSlot1").transform.GetChild(0).gameObject.name);
            decklist[2] = (GameObject.Find("DSlot2").transform.GetChild(0).gameObject.name);
            decklist[3] = (GameObject.Find("DSlot3").transform.GetChild(0).gameObject.name);
            decklist[4] = (GameObject.Find("DSlot4").transform.GetChild(0).gameObject.name);
        }

        public void DeckBackButton()        //DECK패널에 있는 BACK버튼을 눌렀을 경우
        {
            DeleteAllCard();
            deckPanel.gameObject.SetActive(false);
            catdogPanel.gameObject.SetActive(true);
            currentPanel = catdogPanel;
        }

        public void TopPanelBackButton()    //TOP패널에 있는 BACK버튼을 눌렀을 경우
        {
            if (currentPanel==deckPanel)
                DeleteAllCard();
            if (currentPanel.gameObject.activeSelf)
                currentPanel.gameObject.SetActive(false);
        }

        void DeleteAllCard()                //DECK패널에 있는 카드들 삭제, 텍스트초기화(예정)
        {
            for(int i=0; i < 10; i++)
            {
                Destroy(allcard[i]);
            }
        }






    }
}
