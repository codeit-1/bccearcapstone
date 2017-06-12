using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * @author: svenkatesan
 * this class is only for the use of each block
 */

public class BlockScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
                                IBeginDragHandler, IDragHandler, IEndDragHandler {

    public AudioClip oColideSound;
    Material oMat;
  
    // used by action script to determine the state 
    public enum BlockState {
        stacked,
        dragged,
        dropped,
        targetted,
    }
    public static BlockState oBlockState;

    private Vector3 screenPoint;
    private Vector3 offset;

    // Use this for initialization
    void Start () {
         // oBlockState = BlockState.stacked;
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hitInfo;
        if (Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out hitInfo,
                20.0f,
                Physics.DefaultRaycastLayers))
        {
            // If the Raycast has succeeded and hit a hologram
            // hitInfo's point represents the position being gazed at
            // hitInfo's collider GameObject represents the hologram being gazed at
        }

    }

    public static explicit operator BlockScript(GameObject v)
    {
        if (v != null)
            return (BlockScript)v;
        else
            return null;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        oMat = this.GetComponent<MeshRenderer>().material;
        oMat.color = Color.yellow;

        EventSystem.current.SetSelectedGameObject(this.gameObject);

        oBlockState = BlockState.targetted;

    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        oMat = this.GetComponent<MeshRenderer>().material;
        oMat.color = Color.white;

        EventSystem.current.SetSelectedGameObject(null);

        oBlockState = BlockState.stacked;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startMove();
    }

    public void OnDrag(PointerEventData eventData)
    {
        move();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // this.GetComponent<AudioSource>().PlayOneShot(this.oColideSound);
    }

    public void startMove() {

        screenPoint = Camera.main.WorldToScreenPoint(
                        gameObject.transform.position);
        offset = gameObject.transform.position -
                    Camera.main.ScreenToWorldPoint(
                        new Vector3(
                            Input.mousePosition.x,
                            Input.mousePosition.y,
                            screenPoint.z));

        //print("screenpoint " + screenPoint);
        //print("offset " + offset);

    }

    public void move() {
        Vector3 curScreenPoint = new Vector3(
                                    Input.mousePosition.x,
                                    Input.mousePosition.y,
                                    screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
        

        //print("curPosition " + curPosition);
    }


    public void OnCollisionEnter(Collision collision) {
        this.GetComponent<AudioSource>().PlayOneShot(this.oColideSound);
    }


}
