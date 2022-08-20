using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectManipulator : MonoBehaviour {
    [SerializeField] private GameObject lightBoxx;
    [SerializeField] private GameObject myPath;
    private Transform selected;
    // Start is called before the first frame update
    void Start() {
        Debug.Log("starting");
    }
    
    void Update() {
        //if (Input.GetKeyDown(KeyCode.B) && (NOfObjects <Scenemanager.Instance.GetMaxChargingCount())) {
        //    NOfObjects++;
        //    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    //Instantiate(lightBoxx, new Vector3(mousePos.x,mousePos.y,0), Quaternion.identity);
        //    Instantiate(lightBoxx, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity,myPath.transform);
        //}
        Debug.Log("Scenemanager.Instance.GetMaxChargingCount()"+Scenemanager.Instance.GetMaxChargingCount());
        if (Input.GetMouseButtonDown(0) && (Scenemanager.Instance.GetNoOfCharge() < Scenemanager.Instance.GetMaxChargingCount())) {
            Scenemanager.Instance.IncrementNoOfCharge();
           // Debug.Log("NOOBJECT" + NOfObjects);
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Instantiate(lightBoxx, new Vector3(mousePos.x,mousePos.y,0), Quaternion.identity);
            Instantiate(lightBoxx, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity, myPath.transform);
        }
        // only in editor mode 
        if ((Input.GetMouseButtonDown(0)&& (Scenemanager.Instance.GetInEditor()))){
            Debug.Log(Scenemanager.Instance.GetInEditor());
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePosVec2 = new Vector2(mousePos.x, mousePos.y);
            foreach (Transform child in myPath.transform) {
                var target = new Vector2(child.position.x, child.position.y);
                if ((target - mousePosVec2).magnitude <= 0.35) {
                    selected = child;
                    break;
                } else {
                    selected = null;
                }
            } 
        }
        if (Input.GetMouseButtonUp(0)) {
            if (selected != null) selected = null;
        }

        if (selected != null) {
            // Cube selected, move it?
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selected.position = new Vector3 (mousePos.x,mousePos.y,0);
        }
    }
}
