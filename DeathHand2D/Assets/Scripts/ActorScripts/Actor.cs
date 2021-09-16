 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public GameObject background;
    [HideInInspector]
    public Vector2 backgroundSize;
    public static Vector2 mapSizeMin = new Vector2(2.0f, 1.0f);
    public static Vector2 mapSizeMax = new Vector2(3840.4f, 1620.2f);

    // Start is called before the first frame update
    void Start()
    {
        Vector2 spriteSize = background.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 localSpriteSize = spriteSize / background.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        backgroundSize = localSpriteSize * background.transform.lossyScale;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector2 GetSpriteSize(GameObject _target)
    {
        Vector2 worldSize = Vector2.zero;
        if(_target.GetComponent<SpriteRenderer>())
        {
            Vector2 spriteSize = _target.GetComponent<SpriteRenderer>().sprite.rect.size;
            Vector2 localSpriteSize = spriteSize / _target.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            worldSize = localSpriteSize * _target.transform.lossyScale;
        }

        return worldSize;
    }
}
