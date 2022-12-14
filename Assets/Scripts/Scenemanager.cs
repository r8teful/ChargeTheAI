using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour {
    [SerializeField] private GameObject AI;
    [SerializeField] private int[] maxChargingCount;
    private objectManipulator manip;
    public static Scenemanager Instance;
    private bool inEditor = true;
    private int noOfCharge;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    private void Start() {
        manip = FindObjectOfType<objectManipulator>();
    }
    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNextLevel() {
        inEditor = true;
        noOfCharge = 0;
        Destroy(manip);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        manip = FindObjectOfType<objectManipulator>();
    }
    public int GetCurrentSceneNumber() {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public void StartLevel() {
        inEditor = false;
      
        Instantiate(AI);
        manip = FindObjectOfType<objectManipulator>();
    }
    public void EnterEditor() {
        inEditor = true;
       // Debug.Log("BEFORE"+ FindObjectOfType<AIController>());
        Destroy(FindObjectOfType<AIController>().gameObject);
    }

    public bool GetInEditor(){
        return inEditor;
    }

    // Gets max charging count for the current scene
    public int GetMaxChargingCount() {
        return maxChargingCount[SceneManager.GetActiveScene().buildIndex];
    }
    public int GetNoOfCharge() {
        return noOfCharge;
    }
    public void IncrementNoOfCharge() {
        noOfCharge++;
    }
}