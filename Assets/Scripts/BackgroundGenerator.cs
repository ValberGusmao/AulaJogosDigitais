using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    Sprite spr;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 viewport = CameraController.instance.WorldToViewportPoint(pos);

        if (viewport.x < 0 || viewport.x > 1)
        {
            BackgroundGenerator clone = Instantiate(this, transform);
            Vector3 nPos = new Vector3 (spr.rect.width * transform.localScale.x / 100, 0, 0);
            clone.transform.position = pos + nPos;
            clone.transform.localScale = new Vector3(1, 1,1);
            Destroy(this);
            Debug.Break();
        }
    }
}
