using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectMgr : MonoBehaviour
{
    private static PlayerEffectMgr instance = null;

    public static PlayerEffectMgr PlayerEffect
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<PlayerEffectMgr>();
                if(obj == null)
                    instance = new GameObject().AddComponent<PlayerEffectMgr>();
                else
                    instance = obj;
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject attack_Effect;
    public GameObject Attack_Effect { get { return attack_Effect; } }

    [SerializeField]
    private GameObject skill01_Effect;
    public GameObject Skill01_Effect { get { return skill01_Effect; } }

    [SerializeField]
    private GameObject skill02_Effect;
    public GameObject Skill02_Effect { get { return skill02_Effect; } }

    [SerializeField]
    private GameObject skill03_Effect;
    public GameObject Skill03_Effect { get { return skill03_Effect; } }

    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }


    public IEnumerator PlayEffect(GameObject effectPrefab)
    {
        GameObject effectObj = Instantiate(effectPrefab, player.transform.position, player.transform.rotation, player.transform);
        yield return new WaitForSeconds(1.0f);
        Destroy(effectObj);
    }
}
