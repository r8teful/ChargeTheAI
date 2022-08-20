using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class topCollision : MonoBehaviour
{
    private bool hit,g;
    private float hitCoord, yTarget,vel, sinceLast, sinceLast2,sinceCharge;
    private int index; 
    private List<Vector2> waypoints;
    private Transform pathHolder;
    private Vector2 curW, deathPos;
    private AIController myParent;
    private void OnDrawGizmos() {
        if (pathHolder != null) {
            Vector2 startPosition = pathHolder.GetChild(0).position;
            Vector2 previousPosition = startPosition;
            foreach (Transform waypoint in pathHolder) {
                Gizmos.DrawCube(waypoint.position, Vector3.one * .1f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
        }
    }
    void Start() {
        pathHolder = GameObject.Find("Path").transform;
        myParent = transform.parent.GetComponent<AIController>();
        waypoints = new List<Vector2>();
        sinceCharge = Time.time; // Start with full battery 
        // SetRightOrder();
        SetRightSibling();
        PopulateWayPoints();
    }

    private void SetRightSibling() {
        Transform temp;
        // Bubble sort from https://www.tutorialspoint.com/Bubble-Sort-program-in-Chash
        for (int j = 0; j <= pathHolder.childCount - 2; j++) {
            for (int i = 0; i <= pathHolder.childCount - 2; i++) {
                if (pathHolder.GetChild(i).position.x > pathHolder.GetChild(i + 1).position.x) {
                    temp = pathHolder.GetChild(i + 1);
                    pathHolder.GetChild(i + 1).SetSiblingIndex(pathHolder.GetChild(i).GetSiblingIndex());
                    pathHolder.GetChild(i).SetSiblingIndex(temp.GetSiblingIndex());
                }
            }
        }
    }
    private void PopulateWayPoints() {
        for (int i = 0; i < pathHolder.childCount; i++) {
            waypoints.Add(pathHolder.GetChild(i).position);
        }
        yTarget = GetYTarget(0);
        index = -1;
    }

    void Update() {
       // Debug.Log(Time.time);
        if ((index >= -1) && (index <= waypoints.Count) && (vel != myParent.GetVel().x) && !myParent.GetNothingWrong()) {
            if (Time.time - sinceLast > 0.1) {
                if (myParent.GetVel().x == 0f) {        
                    StartCoroutine(ExecuteAfterTime(0.01f));
                } else {
                    if (myParent.GetVel().x > 0) index += 1;
                    if (myParent.GetVel().x < 0) index -= 1;
                    // Dont let out of bounds index make an error
                    if ((index > 0) && (index < waypoints.Count)) yTarget = GetYTarget(index);
                }
            }
            sinceLast = Time.time;
        }
        vel = myParent.GetVel().x;
        if (transform.localPosition.y <= 0.5) {
            // Don't go into bottom layer
            transform.localPosition = new Vector2(transform.localPosition.x, 0.505f);
        }
        // Get next point in list / previous if it's going backwards
        if ((curW.x - transform.position.x < 0.1) && (curW.x - transform.position.x > -0.1)) {
            // Dont let out of bounds index make an error
            if ((index < waypoints.Count) && (index >= 0)) yTarget = GetYTarget(index);
            if (myParent.GetVel().x > 0) {
                // Just allow increase 1 above index bounds
                if (index <= waypoints.Count) {
                    if (Time.time - sinceLast2 > 0.1) index++;
                }
                sinceLast2 = Time.time;
            } else if ((index >= 0 ) && (myParent.GetVel().x <= -1) ) {
                // Going backwardas so decrease 
                if (Time.time - sinceLast2 > 0.1) index--;
            }
            sinceLast2 = Time.time;
        }
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, yTarget), myParent.GetCraneSpeed() * Time.deltaTime);

        if ((Time.time - sinceCharge > 4)&& !(g)) {
            // GAME OVER Stop the AI
            deathPos = transform.position;
            g = true;
           
           Debug.Log("GAME OVER");
        }
        if (g) {
            transform.position = deathPos;
            myParent.Dead();
        }
       
    }
    IEnumerator ExecuteAfterTime(float time) {
        yield return new WaitForSeconds(time);
       
        if (myParent.GetVel().x > 0) index += 2;
        if (myParent.GetVel().x < 0) index -= 2;
        // index = index + 2;
        // Dont let out of bounds index make an error
        if ((index > 0) && (index < waypoints.Count)) yTarget = GetYTarget(index);

    }
    private float GetYTarget(int i) {
        //Debug.Log("Index: " + index + " yTarget: " + yTarget);
        curW = waypoints[i];
        var dirVec = (curW - new Vector2(transform.position.x, transform.position.y));
        return transform.position.y + dirVec.y;

    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Charge")) {
            sinceCharge = Time.time;
        }
        if (collision.gameObject.CompareTag("NextLVL")) {
            Scenemanager.Instance.LoadNextLevel();
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision) {
    //    var c = collision.transform;
    //    var t = transform.position;
    //    if ((c.tag == "Ground") && (((c.position.y > t.y)) ||((c.position.y < t.y))) &&(c.position.x - t.x > -c.transform.localScale.x/2 - 0.5  && c.position.x - t.x < c.transform.localScale.x / 2 +0.5)) {
    //        // Hit the ceiling 
    //        Debug.Log("yas");
    //        hitCoord = transform.position.y;
    //        hit = true;
    //    } else {
    //        hit = false;
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision) {
    //    hit = false;
    //}
    public bool getHit() {
        return hit;
    }
    public float UntillDeath() {
        return (Time.time - sinceCharge);
    }
}
