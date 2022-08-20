using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class topCollision : MonoBehaviour {
    private float yTarget, sinceLast, sinceCharge;
    private int index; 
    private List<Vector2> waypoints;
    private Transform pathHolder;
    private Vector2 curW;
    private AIController myParent;
    public static event Action Death;
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
        Debug.Log("Instantiating AICONTROLLER");
        pathHolder = GameObject.Find("Path").transform;
        myParent = transform.parent.GetComponent<AIController>();
        waypoints = new List<Vector2>();
        AIController.ChangedDirection += DirectionChanged;

        sinceCharge = Time.time; // Start with full battery 
        // SetRightOrder();
        SetRightSibling();
        PopulateWayPoints();
    }
    private void OnDestroy() {
        AIController.ChangedDirection -= DirectionChanged;
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
        index = 0;
    }
    void Update() {
        //Debug.Log(transform.gameObject);
        if (transform.localPosition.y <= 0.5) {
            // Don't go into bottom layer
            transform.localPosition = new Vector2(transform.localPosition.x, 0.505f);
        }
        // Get next point in list / previous if it's going backwards
        if (Mathf.Abs(curW.x - transform.position.x) < 0.1f) { // Check if AI is close to the target in the x axis
            // Dont let out of bounds index make an error
            if ((index < waypoints.Count) && (index >= 0)) yTarget = GetYTarget(index);
            if (myParent.GetDir() >= 1) {
                // Going forwards
                // Just allow increase 1 above index bounds. Also dont let it go up two points in one frame
                if ((index <= waypoints.Count) && (Time.time - sinceLast > 0.1)) {
                    Debug.Log("INcreasing");
                    index++;
                }
            } else if ((index >= 0) && (myParent.GetDir() <= -1)) {
                // Going backwardas so decrease 
                if (Time.time - sinceLast > 0.1) index--;
            }
            sinceLast = Time.time;
        }
        // Update position to next target
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, yTarget), myParent.craneSpeed * Time.deltaTime);

        if (Time.time - sinceCharge > 4) {
            // GAME OVER Stop the AI
            Death?.Invoke();
            myParent.craneSpeed = 0;
        }
    }
    private void DirectionChanged() {
        Debug.Log("Event!");
        if ((index >= -1) && (index <= waypoints.Count)) {
            if (myParent.GetDir() > 0) index += 1;
            if (myParent.GetDir() < 0) index -= 1;
            // Dont let out of bounds index make an error
            if ((index >= 0) && (index < waypoints.Count)) yTarget = GetYTarget(index);
            }
    }
    private float GetYTarget(int i) {
        curW = waypoints[i];
        //Debug.Log($"curW: {curW}");
        //Debug.Log($"transform.position.x: {transform.position.x}");
        //Debug.Log($"transform.position.y: {transform.position.y}");
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
    public float UntillDeath() {
        return (Time.time - sinceCharge);
    }
}
