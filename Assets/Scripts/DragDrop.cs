using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private Logic_15 logic;

    public float putBackSpeed = 10f;
    public bool canTouch;
    private bool isMouseDragging;
    Vector3 offset;
    private float distance;
    private float yCoord;

    private GameObject target;

    private void Start()
    {
        logic = FindObjectOfType<Logic_15>();
    }


    void Update()
    {
        if (logic.gameOver && isMouseDragging)
        {
            isMouseDragging = false;
            StartCoroutine(PutBack());
            logic.isDrag = false;
        }
        
        ///// ON CLICK /////
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }

        ///// ON HOLD /////
        if (isMouseDragging)
        {
            OnMove();
        }


        ///// ON RELEASE /////
        if (Input.GetMouseButtonUp(0))
        {
            if (isMouseDragging)
            {
                OnRelease();
            }
        }

    } // END OF UPDATE

    private void OnClick()
    {
        target = ReturnClickedObject();
        if (target != null && canTouch && !target.GetComponent<Candy>().isFixed)
        {
            isMouseDragging = true;
            OnMove();
        }
    }

    private void OnMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Vector3 newTargetPosition = ray.origin + ray.direction * distance + offset;

        float extraDistance = Vector3.Distance(target.transform.position, newTargetPosition);

        if (newTargetPosition.y < target.transform.position.y)
            extraDistance = -extraDistance;

        Vector3 position = ray.origin + ray.direction * (distance + extraDistance) + offset;

        target.transform.position = new Vector3(position.x, yCoord, position.z);

        logic.isDrag = true;
    }

    private void OnRelease()
    {
        isMouseDragging = false;

        if (!target.GetComponent<Candy>().CheckFix() && logic.isDrag)
        {
            canTouch = false;
            StartCoroutine(PutBack());
        }

        logic.isDrag = false;
    }

    // Move candy back to its place smoothly
    IEnumerator PutBack()
    {
        GameObject toMove = target;
        while ((toMove.transform.position - toMove.GetComponent<Candy>().spawnPosition.position).magnitude > 0.5f)
        {
            Vector3 direction = (toMove.transform.position - toMove.GetComponent<Candy>().spawnPosition.position).normalized;
            toMove.transform.position -= direction * putBackSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        toMove.transform.position = toMove.GetComponent<Candy>().spawnPosition.position;
        if (!logic.gameOver)
            canTouch = true;
    }

    private GameObject ReturnClickedObject()
    {
        RaycastHit hit;
        GameObject targetObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Draggable") && !logic.gameCompleted)
            {
                targetObject = hit.collider.gameObject;
                offset = targetObject.transform.position - hit.point;
                distance = hit.distance;
                yCoord = 1f;
            }

        }
        return targetObject;
    }
}
