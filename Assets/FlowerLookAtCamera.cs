using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerLookAtCamera : MonoBehaviour
{
    [SerializeField]
    List<Sprite> m_Sprites;
    private void Awake()
    {
        ChangeSprite();
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
    public void ChangeSprite() 
    {
        var index = Random.Range(0, m_Sprites.Count);
        gameObject.GetComponent<SpriteRenderer>().sprite = m_Sprites[index];
    }
}
