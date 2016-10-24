using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD_Keys : MonoBehaviour {

    public GameObject masterController;

    GameObject[] keys;
    GameObject[] emptyKeys;
    LevelManager levelManager;
    int defaultLayerIndex;
    int UILayerIndex;

	// Use this for initialization
	void Start () {
        levelManager = masterController.GetComponent<LevelManager>();
        Transform childKeys = transform.FindChild("Keys");
        Transform childEmptyKeys = transform.FindChild("Keys_Empty");

        List<GameObject> keyList = new List<GameObject>();
        List<GameObject> emptyKeyList = new List<GameObject>();
        for (int i = 0; i < childKeys.childCount; i++)
        {
            keyList.Add(childKeys.GetChild(i).gameObject);
            emptyKeyList.Add(childEmptyKeys.GetChild(i).gameObject);
        }
        keys = keyList.ToArray();
        emptyKeys = emptyKeyList.ToArray();
                
        defaultLayerIndex = LayerMask.NameToLayer("Default")+1;
        UILayerIndex = LayerMask.NameToLayer("UI");
        clearKeys();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	    for(int i = 0; i < levelManager.currentKeys; i++)
        {
            emptyKeys[i].transform.GetChild(0).gameObject.layer = defaultLayerIndex;
            keys[i].transform.GetChild(0).gameObject.layer = UILayerIndex;
        }
	}

    public void clearKeys()
    {
        foreach (GameObject key in keys)
            key.transform.GetChild(0).gameObject.layer = defaultLayerIndex;
        foreach (GameObject key in emptyKeys)
            key.transform.GetChild(0).gameObject.layer = defaultLayerIndex;
    }

    public void resetKeys(int keysRequired)
    {
        for(int i = 0; i < keysRequired; i++)
        {
            emptyKeys[i].transform.GetChild(0).gameObject.layer = UILayerIndex;
        }
    }
}
