using UnityEngine;
using System.Collections;

public class Crop : MonoBehaviour
{
    //public Resources resources;
    public FieldElement field;
    public int foodPoints;
    private void Start()
    {
        var pos = transform.position;
        this.transform.position = new Vector3(pos.x, pos.y + 0.03f, pos.z);
    }
    void Update()
    {
        //this.transform.position = Vector3.up * Mathf.Cos(Time.time);
        transform.Rotate(Vector3.forward * Time.deltaTime * 50);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform == this.transform || hit.transform == field.transform)
                {
                    FindObjectOfType<ResourcesCount>().GetComponent<ResourcesCount>().food += foodPoints;
                    field.planted = false;
                    Destroy(this.gameObject);
                }
            }
        }
    }
}