using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System; // events -> https://www.youtube.com/watch?v=70PcP_uPuUc
public class AIController : MonoBehaviour {
    private float direction;
    private Rigidbody2D rb2;
    public float moveSpeed, craneSpeed;
    public static event Action ChangedDirection;
    // Start is called before the first frame update
    void Start() {
        rb2 = GetComponent<Rigidbody2D>();
        direction = 1;
        topCollision.Death += Dead;
        Debug.Log("Instantiating AICONTROLLER");
    }
    private void OnDestroy() {
        topCollision.Death -= Dead;
    }
    private void FixedUpdate() {
       // Move the AI 
       rb2.velocity = new Vector2((direction) * moveSpeed, rb2.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            // Change direction
            direction = -direction;
            ChangedDirection?.Invoke();
        }
    }
    public float GetDir() {
        return direction;
    }
    public void Dead() {
        moveSpeed = 0;
    }
}