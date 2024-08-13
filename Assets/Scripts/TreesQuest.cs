using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TreesQuest : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject placeIndicatorPrefab;
    [SerializeField] private GameObject treesPrefab;

    private GameObject placeIndicator;
    private GameObject trees;

    private List<ARRaycastHit> hitResults = new();

    private void Start()
    {
        placeIndicator = Instantiate(placeIndicatorPrefab);
        placeIndicator.SetActive(false);
        StartCoroutine(SpawnIndicatorMoving());
    }

    private IEnumerator SpawnIndicatorMoving()
    {
        var waitable = new WaitForEndOfFrame();
        while (!placeIndicator.IsUnityNull())
        {
            var ray = new Vector2(Screen.width / 2, Screen.height / 2);

            if (raycastManager.Raycast(ray, hitResults, TrackableType.Planes))
            {
                Pose hitPose = hitResults[0].pose;
                placeIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                if (!placeIndicator.activeInHierarchy)
                    placeIndicator.SetActive(true);
            }
            yield return waitable;
        }
    }

    public void SpawnTrees()
    {
        placeIndicator.SetActive(false);
        trees = Instantiate(treesPrefab);
        trees.transform.SetPositionAndRotation(placeIndicator.transform.position, placeIndicator.transform.rotation);
        Destroy(placeIndicator);
    }

    public void EndQuest()
    {
        Application.Quit();
    }
}
