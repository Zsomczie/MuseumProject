using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
struct ArObject
{
    public GameObject obj;
    public bool isTracked;
    public bool isPainting;

    public ArObject(GameObject gameObject, bool track, bool painting) 
    {
        obj = gameObject;
        isTracked = track;
        isPainting = painting;
    }
}
public class ImageTracker : MonoBehaviour
{

    private ARTrackedImageManager trackedImages;
    public GameObject[] ArPrefabs;
    public string[] scenes;

    public TMP_Text text;
    public TMP_Text text2;


    bool isItTracked;
    bool hasbeenreset;

    List<ArObject> ArObjects = new List<ArObject>();

    private void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedimage in eventArgs.added)
        {
            //if (!trackedimage.referenceImage.name.Contains("Scene"))
            //{
                foreach (var ARPrefab in ArPrefabs)
                {
                    if (trackedimage.referenceImage.name == ARPrefab.name&&trackedimage.referenceImage.name.Contains("Painting"))
                    {
                        var newPrefab = Instantiate(ARPrefab, trackedimage.transform);
                        ArObjects.Add(new ArObject(newPrefab, true,true));
                    }
                    else if (trackedimage.referenceImage.name == ARPrefab.name && !trackedimage.referenceImage.name.Contains("Painting"))
                    {
                        var newPrefab = Instantiate(ARPrefab, trackedimage.transform);
                    var permanentNewPrefab = Instantiate(ARPrefab, newPrefab.transform);
                    permanentNewPrefab.GetComponent<SpriteRenderer>().sprite=newPrefab.GetComponent<SpriteRenderer>().sprite;
                    ArObjects.Add(new ArObject(newPrefab, true, false));
                    }
                }
            //}
            //else
            //{
            //    foreach (var scene in scenes) 
            //    {
            //        if (trackedimage.referenceImage.name.Contains(scene))
            //        {
            //            SceneManager.LoadSceneAsync(scene,LoadSceneMode.Additive);
            //        }
            //    }
            //}
            
        }
        foreach (var trackedimage in eventArgs.updated)
        {
            isItTracked = (trackedimage.trackingState == TrackingState.Tracking);
            foreach (var arObject in ArObjects)
            {
                //if (!trackedimage.referenceImage.name.Contains("Scene"))
                //{
                if (arObject.obj.name.Contains(trackedimage.referenceImage.name))
                {
                    if (isItTracked && !arObject.isPainting)
                    {
                        if (trackedimage.transform.GetChild(0).childCount > 0)
                        {
                            arObject.obj.SetActive(true);
                        }

                    }
                    //else if (!isItTracked && !arObject.isPainting)
                    //{
                    //    if (trackedimage.transform.GetChild(0).childCount>0)
                    //    {
                    //        var permanent = Instantiate(arObject.obj, arObject.obj.transform.position, arObject.obj.transform.rotation);
                    //        permanent.transform.SetParent(null);
                    //        permanent.transform.position=arObject.obj.transform.position;
                    //    }
                    //    //arObject.obj.transform.localScale = new Vector3(0, 0, 0);
                    //    //int i = ArObjects.IndexOf(arObject);

                    //}
                    else if (isItTracked && arObject.isPainting)
                    {
                        if (!hasbeenreset)
                        {
                            //arObject.obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                            hasbeenreset = true;
                        }
                        arObject.obj.SetActive(true);
                    }
                    else if (!isItTracked && arObject.isPainting)
                    {
                        arObject.obj.SetActive(false);
                        hasbeenreset = false;
                    }

                }
                //}
                //else if (!isItTracked)
                //{
                //    foreach (var scene in scenes)
                //    {
                //        if (trackedimage.referenceImage.name.Contains(scene))
                //        {
                //            SceneManager.UnloadSceneAsync(scene,UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                //        }
                //    }
                //}
                //else if (isItTracked)
                //{
                //    foreach (var scene in scenes)
                //    {
                //        if (trackedimage.referenceImage.name.Contains(scene))
                //        {
                //            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                //        }
                //    }
                //}
            }
        }
        if (eventArgs.updated.Count > 0)
        {
            if (Input.touchCount > 0 && ArObjects.Count > 0)
            {
                foreach (var trackedimage in eventArgs.updated)
                {
                    
                    Debug.Log(string.Join(',', ArObjects));
                    text.text = "";
                    foreach (var arObject in ArObjects)
                    {
                        if (arObject.obj.transform.localScale.y == 0f && !arObject.isPainting&&trackedimage.trackingState==TrackingState.Tracking&& arObject.obj.name.Contains(trackedimage.referenceImage.name))
                        {

                            gameObject.transform.DOScale(0.3f, 1f).OnComplete(() => arObject.obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f));

                            //gameobject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        }
                        else if (arObject.obj.transform.localScale.y != 0f && !arObject.isPainting && trackedimage.trackingState == TrackingState.Tracking&& arObject.obj.name.Contains(trackedimage.referenceImage.name))
                        {
                            gameObject.transform.DOScale(0, 1f).OnComplete(() => arObject.obj.transform.localScale = new Vector3(0, 0, 0));

                            //gameobject.transform.localScale = new Vector3(0, 0, 0);

                        }
                    }
                }
            }
        }

    }

    //private void Update()
    //{
          
    //    if (Input.touchCount > 0&&ArObjects.Count>0)
    //    {
    //        Debug.Log(string.Join(',', ArObjects));
    //        text.text = ArObjects[0].isTracked.ToString();
    //        foreach (var arObject in ArObjects)
    //        {

    //            if (arObject.obj.transform.localScale.y == 0f&&!arObject.isPainting)
    //            {

    //                gameObject.transform.DOScale(0.3f, 1f).OnComplete(() => arObject.obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f));
                    
    //                //gameobject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    //            }
    //            else if (arObject.obj.transform.localScale.y != 0f && !arObject.isPainting)
    //            {
    //                gameObject.transform.DOScale(0, 1f).OnComplete(() => arObject.obj.transform.localScale = new Vector3(0, 0, 0));

    //                //gameobject.transform.localScale = new Vector3(0, 0, 0);

    //            }
    //        }
    //    }


    //}
}
