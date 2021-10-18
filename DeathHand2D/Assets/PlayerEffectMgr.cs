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
    private List<GameObject> attack_Effect;
    public List<GameObject> Attack_Effect { get { return attack_Effect; } }

    [SerializeField]
    private List<GameObject> skill01_Effect;
    public List<GameObject> Skill01_Effect { get { return skill01_Effect; } }

    [SerializeField]
    private List<GameObject> skill02_Effect;
    public List<GameObject> Skill02_Effect { get { return skill02_Effect; } }

    [SerializeField]
    private List<GameObject> skill03_Effect;
    public List<GameObject> Skill03_Effect { get { return skill03_Effect; } }

    [SerializeField]
    private List<GameObject> run_Effect;
    public List<GameObject> Run_Effect { get { return run_Effect; } }

    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }


    public void PlayEffect(GameObject effect)
    {

        GameObject obj = Instantiate(
            effect, 
            player.transform.position,
            new Quaternion(effect.transform.rotation.x, player.transform.rotation.y, effect.transform.rotation.z, effect.transform.rotation.w),
            player.transform
            );
        Destroy(obj, obj.GetComponent<ParticleSystem>().duration);

    }
}
