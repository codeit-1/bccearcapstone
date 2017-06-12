using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using UnityEngine.EventSystems;
// added for speach
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class ActionScript : MonoBehaviour {

    BlockScript oBlockScript;

    public float hitForce;

    private GestureRecognizer gestureRecognizer;
    //public AudioClip airTapSound;

    //private Vector3 screenPoint;
    //private Vector3 offset;

    //added for speach
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    private KeywordRecognizer keywordRecongnizer;

    // Use this for initialization
    void Start () {
        Debug.Log("Action Script started");
        
        //Material oMat = gameObject.GetComponent<Renderer>().material;
        //oMat.color = new Color(1F, 1F, 1F, 1F);

        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);
        gestureRecognizer.ManipulationStartedEvent += GestureRecognizer_ManipulationStartedEvent;
        gestureRecognizer.ManipulationUpdatedEvent += GestureRecognizer_ManipulationUpdatedEvent;
        gestureRecognizer.ManipulationCompletedEvent += GestureRecognizer_ManipulationCompletedEvent;
        gestureRecognizer.StartCapturingGestures();

        // added for speach
        keywords.Add("Jenga Restart", () => {
            Debug.Log("Restarting");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });

        keywordRecongnizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecongnizer.OnPhraseRecognized += KeywordRecongnizer_OnPhraseRecognized;
        keywordRecongnizer.Start();
        // end added for speach

    }

    private void GestureRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        /*
        Debug.Log("enter into completed");
        GameObject oCurrentGameObject = EventSystem.current.currentSelectedGameObject;

        if (oCurrentGameObject != null)
        {
            oBlockScript = oCurrentGameObject.GetComponent<BlockScript>();
            Debug.Log("got blockscript");
            if (oBlockScript != null)
            {
                oBlockScript.move();
                Debug.Log("startmove blockscript");
            }
            else
                Debug.Log("block script null");
        }
        else
            Debug.Log("current game object is null");
        */
    }

    private void GestureRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        Debug.Log("enter into updated");
        GameObject oCurrentGameObject = EventSystem.current.currentSelectedGameObject;

        if (oCurrentGameObject != null)
        {
            oBlockScript = oCurrentGameObject.GetComponent<BlockScript>();
            Debug.Log("got blockscript");
            if (oBlockScript != null)
            {
                //oBlockScript.startMove();
                //Debug.Log("start move called");
                oBlockScript.move();
                Debug.Log("move called");
            }
            else
                Debug.Log("block script null");
        }
        else
            Debug.Log("current game object is null");
    }

    private void GestureRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        Debug.Log("enter started");
        GameObject oCurrentGameObject = EventSystem.current.currentSelectedGameObject;

        if (oCurrentGameObject !=null ) {
            oBlockScript = oCurrentGameObject.GetComponent<BlockScript>();
            Debug.Log("got blockscript");
            if (oBlockScript != null) {
                oBlockScript.startMove();
                Debug.Log("startmove blockscript");
            }
            else
                Debug.Log("block script null");
        }
        else
            Debug.Log("current game object is null");
    }

    //private void GestureRecognizer_HoldEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    //{
    //    this.GetComponent<AudioSource>().PlayOneShot(this.airTapSound);
    //    ApplyForce(new Ray(Camera.main.transform.position, Camera.main.transform.forward));
    //}

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    ApplyForce(ray);
        //}
    }

    private void ApplyForce(Ray ray)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            var targetRigidbody = hitInfo.collider.GetComponent<Rigidbody>();
            targetRigidbody.AddForceAtPosition(hitForce * ray.direction.normalized, hitInfo.point);
        }
    }

    // added for speach
    private void KeywordRecongnizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action action;
        if (keywords.TryGetValue(args.text, out action))
        {
            action();
        }
    }

}
