using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasController : MonoBehaviour {
    [SerializeField] private Text chargeNumber;
    [SerializeField] private Button StartButton;

    private void Start() {
    }
    public void OnPlayClicked() {
        var s = Scenemanager.Instance;
        // Check if I'm in the editor or not, check if I've placed all my charging stations 
        if (s.GetInEditor() && (s.GetMaxChargingCount() == s.GetNoOfCharge())) {
            s.StartLevel();
        } else {
            s.EnterEditor();
        }
        
    }
    private void Update() {
        Debug.Log("NogObjects: " + Scenemanager.Instance.GetNoOfCharge());
        chargeNumber.text = "x "+ (Scenemanager.Instance.GetMaxChargingCount() - Scenemanager.Instance.GetNoOfCharge()).ToString();
        if (Scenemanager.Instance.GetInEditor() && (Scenemanager.Instance.GetMaxChargingCount() == Scenemanager.Instance.GetNoOfCharge())) {
            StartButton.image.color = Color.green;
            StartButton.GetComponentInChildren<Text>().text = "Start";
        } else if (!Scenemanager.Instance.GetInEditor()){
            StartButton.image.color = Color.red;
            StartButton.GetComponentInChildren<Text>().text = "Try again";
        } else {
            StartButton.image.color = Color.red;
            StartButton.GetComponentInChildren<Text>().text = "Use all items first!";
        }
       // Debug.Log(StartButton.image.color);
    }
}
