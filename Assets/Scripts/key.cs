using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour {

    [SerializeField] private GameObject gate;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
        gate.SetActive(false);
        gameObject.SetActive(false);
        }
    }
}
