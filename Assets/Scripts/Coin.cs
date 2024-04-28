using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AreaEffector2D areaEffector2D;

    // Start is called before the first frame update
    void Start()
    {
        
        areaEffector2D = GetComponent<AreaEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.isTrigger && col.gameObject.CompareTag("Player"))
        {
            GameController.instance.AddPoints(1);
            Destroy(gameObject);
        }

    }
}
