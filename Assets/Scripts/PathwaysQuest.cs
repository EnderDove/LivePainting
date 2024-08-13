using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PathwaysQuest : MonoBehaviour
{
    [SerializeField] private float firstDelay = 10;
    [SerializeField] private float secondDelay = 10;
    [SerializeField] private ARRaycastManager raycastManager;

    [SerializeField] private GameObject placeIndicatorPrefab;
    [SerializeField] private GameObject guyPrefab;
    [SerializeField] private GameObject pathwaysPrefab;
    [SerializeField] private GameObject findText;
    [SerializeField] private GameObject followText;

    private GameObject guy;
    private GameObject placeIndicator;
    private GameObject pathways;
    private List<ARRaycastHit> hitResults = new();

    private AsyncOperation loadSceneOperation;

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

    public void SpawnGuy()
    {
        placeIndicator.SetActive(false);
        guy = Instantiate(guyPrefab);
        guy.transform.SetPositionAndRotation(placeIndicator.transform.position, placeIndicator.transform.rotation);
        Destroy(placeIndicator);
        Invoke(nameof(SpawnPathways), firstDelay);

        //starting speech
    }

    public void SpawnPathways()
    {
        pathways = Instantiate(pathwaysPrefab);
        pathways.transform.position = Vector3.Lerp(Camera.main.transform.position - Vector3.up * Camera.main.transform.position.y, guy.transform.position, 0.5f);
        findText.SetActive(true);
        Destroy(guy);
    }

    public void EndQuest()
    {
        //after quest, call from Chooser
        findText.SetActive(false);
        followText.SetActive(true);
        guy = Instantiate(guyPrefab);
        guy.transform.position = pathways.transform.position;
        guy.transform.LookAt(Camera.main.transform.position - Vector3.up * (Camera.main.transform.position.y - pathways.transform.position.y));
        guy.transform.rotation *= Quaternion.Euler(0, 180, 0);
        loadSceneOperation = SceneManager.LoadSceneAsync("Trees");
        loadSceneOperation.allowSceneActivation = false;

        Invoke(nameof(LoadScene), secondDelay);
    }

    public void LoadScene()
    {
        loadSceneOperation.allowSceneActivation = true;
    }

}