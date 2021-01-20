using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PrefabParent : MonoBehaviour {
#if UNITY_EDITOR

    [SerializeField] private string blockType;

    private GameObject floorParent;
    private GameObject platformsParent;
    private GameObject levelParent;
    private GameObject wallsParent;
    private GameObject enemyParent;

    private GameObject lightingParent;
    private GameObject cameraParent;
    private GameObject coinParent;

    void Start()
    {
        // set _Map GameObject
        GameObject tempMapObject;
        GameObject fetchGameObject = GameObject.Find("_Map");
        if (fetchGameObject == null)
        {
            tempMapObject = new GameObject("_Map");
            tempMapObject.transform.position = Vector3.zero;
        }
        else
        {
            tempMapObject = fetchGameObject;
        }

        // set floorParent
        fetchGameObject = GameObject.Find("_Map/_Floor");
        if (fetchGameObject == null)
        {
            floorParent = new GameObject("_Floor");
            floorParent.transform.parent = tempMapObject.transform;
            floorParent.transform.localPosition = new Vector3(0, -0.5f, 0);
        }
        else
        {
            floorParent = fetchGameObject;
        }

        // set platformsParent
        fetchGameObject = GameObject.Find("_Map/_Floor/_Platforms");
        if (fetchGameObject == null)
        {
            platformsParent = new GameObject("_Platforms");
            platformsParent.transform.parent = floorParent.transform;
            platformsParent.transform.localPosition = Vector3.zero;
        }
        else
        {
            platformsParent = fetchGameObject;
        }

        // set levelParent
        fetchGameObject = GameObject.Find("_Map/_Level");
        if (fetchGameObject == null)
        {
            levelParent = new GameObject("_Level");
            levelParent.transform.parent = tempMapObject.transform;
            levelParent.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        else
        {
            levelParent = fetchGameObject;
        }

        // set wallsParent
        fetchGameObject = GameObject.Find("_Map/_Level/_Walls");
        if (fetchGameObject == null)
        {
            wallsParent = new GameObject("_Walls");
            wallsParent.transform.parent = levelParent.transform;
            wallsParent.transform.localPosition = Vector3.zero;
        }
        else
        {
            wallsParent = fetchGameObject;
        }

        // set enemyParent
        fetchGameObject = GameObject.Find("_Map/_Level/_Enemies");
        if (fetchGameObject == null)
        {
            enemyParent = new GameObject("_Enemies");
            enemyParent.transform.parent = levelParent.transform;
            enemyParent.transform.localPosition = Vector3.zero;
        }
        else
        {
            enemyParent = fetchGameObject;
        }

        // set lightingParent
        fetchGameObject = GameObject.Find("_Lighting");
        if (fetchGameObject == null)
        {
            lightingParent = new GameObject("_Lighting");
            lightingParent.transform.localPosition = new Vector3(0, 50, 0);
        }
        else
        {
            lightingParent = fetchGameObject;
        }

        // set lightingParent
        fetchGameObject = GameObject.Find("_Camera");
        if (fetchGameObject == null)
        {
            cameraParent = new GameObject("_Camera");
            cameraParent.transform.localPosition = new Vector3(0, 10, 0);
        }
        else
        {
            cameraParent = fetchGameObject;
        }

        // set lightingParent
        fetchGameObject = GameObject.Find("_Map/_Level/_Coins");
        if (fetchGameObject == null)
        {
            coinParent = new GameObject("_Coins");
            coinParent.transform.parent = levelParent.transform;
            coinParent.transform.localPosition = Vector3.zero;
        }
        else
        {
            coinParent = fetchGameObject;
        }
    }

    void Update()
    {
        switch (blockType)
        {
            case "Floor":
                if (transform.parent != floorParent.transform)
                    transform.parent = floorParent.transform;
                break;
            case "Platform":
                if (transform.parent != platformsParent.transform)
                    transform.parent = platformsParent.transform;
                break;
            case "Level":
                if (transform.parent != levelParent.transform)
                    transform.parent = levelParent.transform;
                break;
            case "Wall":
                if (transform.parent != wallsParent.transform)
                    transform.parent = wallsParent.transform;
                break;
            case "Enemy":
                if (transform.parent != enemyParent.transform)
                    transform.parent = enemyParent.transform;
                break;
            case "Lighting":
                if (transform.parent != lightingParent.transform)
                {
                    transform.parent = lightingParent.transform;
                    transform.localPosition = Vector3.zero;
                }
                break;
            case "Camera":
                if (transform.parent != cameraParent.transform)
                    transform.parent = cameraParent.transform;
                break;
            case "Coin":
                if (transform.parent != coinParent.transform)
                    transform.parent = coinParent.transform;
                break;
            default:
                Debug.Log(blockType + " is not a correct block type. Valid block types are: Floor, Platform, Level, Wall, Enemy, Lighting, Camera");
                break;
        }
	}
#endif
}
