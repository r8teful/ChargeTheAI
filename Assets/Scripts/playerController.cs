using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class playerController : MonoBehaviour {
    private float inputHor;
    private Rigidbody2D rb2, AIrb2;
    public float moveSpeed, jumpHeight;
    private bool onAI, isDone;
    [SerializeField] private Rigidbody2D AI;
    void Start() {
        rb2 = GetComponent<Rigidbody2D>();
        AIrb2 = AI.GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Left, right input
        inputHor = Input.GetAxis("Horizontal");

        //Jumping
        if (Input.GetKeyDown(KeyCode.W) && isGrounded()) {
            rb2.velocity = new Vector2(rb2.velocity.x, jumpHeight);
        }
    }
    private void FixedUpdate() {
        // Move player depending on imput
        if (!onAI) {
            rb2.velocity = new Vector2((inputHor) * moveSpeed, rb2.velocity.y);
            isDone = false;
        }
        
        if (onAI) {
            if (!isDone) {
                rb2.velocity = new Vector2(0f, 0f);
                isDone = true;
            }
            rb2.velocity = new Vector2((inputHor) * moveSpeed + AIrb2.velocity.x, rb2.velocity.y);
            }
        // Gravity
        if (!isGrounded()) rb2.velocity += Physics.gravity.y * 2f * Vector2.up * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
       if (collision.gameObject.name == "AI Top") {
           // Standing on top of AI
           onAI = true;
       }
        if (collision.CompareTag("NextLVL")) Scenemanager.Instance.LoadNextLevel();
    }
 
   private void OnTriggerExit2D(Collider2D collision) {
       if (collision.gameObject.name == "AI Top") onAI = false;
   }
    private bool isGrounded() {
        return Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 1.1f), new Vector2(.9f,.1f),0f,Vector2.down, 0.1f);
    }
    
}