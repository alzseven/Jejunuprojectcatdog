using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {
    private static ObjectManager instance_ = null;
    public static ObjectManager Instance { get { return instance_; } private set { } }

    public enum ObjectType
    {
        Enemy,
        Player,
    }

    Dictionary<ObjectType, List<GameObject>> objectLists = new Dictionary<ObjectType, List<GameObject>>();
    /// <summary>
    /// 타입별로 그룹화해서, 오브잭트를 저장한다.
    /// </summary>
    /// <param name = "objType"></param>
    /// <param name = "obj"></param>
    public void AddObject(ObjectType objType, GameObject obj)
    {
        if (!objectLists.ContainsKey(objType))
        {
            objectLists[objType] = new List<GameObject>();
        }
        if (!objectLists[objType].Contains(obj))
        {
            objectLists[objType].Add(obj);
        }
    }
    /// <summary>
    /// 특정 타입에서 특정 오브잭트를 삭제한다.
    /// </summary>
    /// <param name = "objType"></param>
    /// <param name = "obj"></param>
    public void RemoveObject(ObjectType objType, GameObject obj)
    {
        if (objectLists[objType].Contains(obj))
        {
            objectLists[objType].Remove(obj);
        }
        Destroy(obj);
    }
    /*public GameObject CreateObject(GameObject parent, GameObject prefab)
    {
        GameObject go = Instantiate(prefab) as GameObject;
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
        }
        return go;
    }
    //Test
    /*public void CreateObjectByTest()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Cat") as GameObject;

        GameObject player = CreateObject(gameObject, playerPrefab);

        AddObject(ObjectType.Player, player);
    }*/
    public ObjectType GetHostileType(ObjectType myObjectType)
    {
        if (myObjectType == ObjectType.Enemy)
        {
            return ObjectType.Player;
        }
        return ObjectType.Enemy;
    }
    public void Dead(GameObject actor)
    {
        UnitAttribute actorAttribute = actor.GetComponent<UnitAttribute>();

        RemoveObject(actorAttribute.ObjectType, actor);

    }
    public GameObject FindFirstEnemy_MKII(GameObject actor)
    {
        //나의 속성(플레이어인지 적인지)를 얻어오기 위해 캐릭터 어트리뷰트를 얻어온다
        //또한 속성은 Player와 Monster_1 프리펩에 설정되 있어야 한다.
        UnitAttribute actorAttribute = actor.GetComponent<UnitAttribute>();
        if (actorAttribute == null)
            return null;

        //액터 입장에서 적대적인 객체타입을 얻어온다.
        ObjectType hostileType = GetHostileType(actorAttribute.ObjectType);
        //Debug.Log(GetHostileType(actorAttribute.ObjectType));

        //해당 타입의 오브잭트가 추가되기 전에 찾게 되는 경우는 못 찾은것으로 처리한다.
        if (!objectLists.ContainsKey(hostileType))
            return null;
        
        //적대적인 타입을 가진 오브젝트 리스트에서 정해준 거리안에 있는 1번째 대상을 반환한다.
        //적대적인 대상을 모두 반환해서, 광역스킬을 사용한다던지 할 수도 있지만, 일단 첫번째 대상만 반환하도록 하자.
        foreach(GameObject obj in objectLists[hostileType])
        {
            UnitAttribute targetAttribute = obj.GetComponent<UnitAttribute>();
            if (targetAttribute == null)
                continue;
            if (targetAttribute.HP <= 0)
                continue;


            float Dis = actor.transform.position.z - obj.transform.position.z;
            if (Dis <= 0)
                Dis = -Dis;
            bool InSameLane = (actor.transform.position.x == obj.transform.position.x);


            //distance = distance + targetAttribute.Radius + actorAttribute.AttackRange;
            if (Dis <= actorAttribute.AttackRange && InSameLane)
            {
                if (obj != null)
                {
                    Debug.Log("Find Enemy:" + obj.name);
                    return obj;
                }
                if (obj == null)
                {
                    Debug.Log("Fail 2 Find Enemy");
                    return obj;
                }
            }
        }
        return null;
    }
    public GameObject CreateObj(GameObject parent, GameObject prefab)
    {
        GameObject go = Instantiate(prefab) as GameObject;

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
        }
        return go;
    }
    //Test
    public void CreateObjByTest()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Cat") as GameObject;

        GameObject player = CreateObj(gameObject, playerPrefab);

        AddObject(ObjectType.Player, player);
    }
    public void CreateDogByTest()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Cat003") as GameObject;

        GameObject player = CreateObj(gameObject, playerPrefab);

        AddObject(ObjectType.Enemy, player);
    }
    public void SandBag()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Dog004") as GameObject;

        GameObject player = CreateObj(gameObject, playerPrefab);

        AddObject(ObjectType.Player, player);
    }
    /*public GameObject FindFirstEnemy(GameObject actor, float distance)
    {
        //나의 속성(플레이어인지 적인지)를 얻어오기 위해 캐릭터 어트리뷰트를 얻어온다
        //또한 속성은 Player와 Monster_1 프리펩에 설정되 있어야 한다.
        CharacterAttribute actorAttribute = actor.GetComponent<CharacterAttribute>();
        if (actorAttribute == null)
            return null;

        //액터 입장에서 적대적인 객체타입을 얻어온다.
        ObjectType hostileType = GetHostileType(actorAttribute.ObjectType);

        //해당 타입의 오브잭트가 추가되기 전에 찾게 되는 경우는 못 찾은것으로 처리한다.
        if (!objectLists.ContainsKey(hostileType))
            return null;

        //적대적인 타입을 가진 오브젝트 리스트에서 정해준 거리안에 있는 1번째 대상을 반환한다.
        //적대적인 대상을 모두 반환해서, 광역스킬을 사용한다던지 할 수도 있지만, 일단 첫번째 대상만 반환하도록 하자.
        foreach (GameObject obj in objectLists[hostileType])
        {
            CharacterAttribute targetAttribute = obj.GetComponent<CharacterAttribute>();
            if (targetAttribute == null)
                continue;
            if (targetAttribute.HP <= 0)
                continue;

            distance = distance + targetAttribute.Radius + actorAttribute.Radius;
            if (Vector3.Distance(actor.transform.position, obj.transform.position) <= distance)
            {
                Debug.Log("Find Enemy:" + obj.name);
                return obj;
            }
        }
        return null;
    }*/
    // Use this for initialization
    public void Awake () {
        instance_ = this;
        //CreateObjectByTest();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
