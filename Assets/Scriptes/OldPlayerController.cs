using UnityEngine;

public class OldPlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject selectObj;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }

        if (Input.GetMouseButton(0))
        {

        }

        if (Input.GetMouseButtonUp(0))
        {

        }
    }


    private void ButtonDownState()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider != null)
            {
                selectObj = hit.collider.gameObject;
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
            }
        }
    }

    private void ButtonIngState()
    {

    }

    private void ButtonUpState()
    {

    }
}
