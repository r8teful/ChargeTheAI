using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System; // events -> https://www.youtube.com/watch?v=70PcP_uPuUc
public class AIController : MonoBehaviour {
    private float craneDirection,direction, hitTime;
    private Rigidbody2D rb2;
    private Transform upperPart;
    public float moveSpeed, craneSpeed;
    private topCollision tc;
    private bool nothingWrong, dead; 
    // Start is called before the first frame update
    void Start() {
        rb2 = GetComponent<Rigidbody2D>();
        upperPart = gameObject.transform.GetChild(0);
        tc = upperPart.GetComponent<topCollision>();
        direction = 1;
    }

    // Update is called once per frame
    void Update() {
            craneDirection = 0;
    }

    private void FixedUpdate() {
       //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
       if (!dead) {
            upperPart.Translate(new Vector3(0, craneDirection, 0) * Time.deltaTime * craneSpeed);
            rb2.velocity = new Vector2((direction) * moveSpeed, rb2.velocity.y);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    if (Time.time - hitTime > 0.01) {
    //        if ((!tc.getHit()) && collision.gameObject.CompareTag("Ground"))  {
    //            direction = -direction;
    //        }
    //    }
    //    hitTime = Time.time;
    //}
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            //Debug.Log("BONK");
            direction = -direction;
        }
        if (collision.gameObject.CompareTag("Player")) {
            nothingWrong = true; 
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            nothingWrong = false;
        }
    }

    public Vector2 GetVel() {
        return rb2.velocity;
    }
    public void Dead() {
        rb2.velocity = Vector2.zero;
        dead = true;
    }
    public float GetCraneSpeed() {
        return craneSpeed;
    }
    public bool GetNothingWrong() {
        return nothingWrong;
    }
}