using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour {

    [SerializeField] private GameObject gate;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
        gate.SetActive(false);
        gameObject.SetActive(false);
        }
    }
}
