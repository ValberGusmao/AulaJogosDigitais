using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LimiteTela : MonoBehaviour
{
    public GameObject jogador;
    // Start is called before the first frame update
    void Start()
    {
        if (jogador == null)
        {
            Debug.LogError("Você esqueceu de adicionar um jogador para objeto "+gameObject.name);
            Debug.Break();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jogador.transform.position.y <= transform.position.y)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
