using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Farming : MonoBehaviour
{
    public bool FarmingMode;
    private RaycastHit mouseHit;
    public Plant[] plants;
    public GameObject[] plantIcons;
    private int currentPlant = 0;
    private Plant plant;
    [Header("Colors")]
    public Color NotOccupied = Color.green;
    public Color Occupied = Color.red;

    public FieldElement fE;
    public Text mode;
    public Clicker clicker;
    private bool clicked;
    private bool released;
    private void Start()
    {
        if(plants[0]!=null)
            plant = plants[0];
    }

    private void Update()
    {
        clicked = clicker.ManoClicked;
        released = clicker.ManoReleased;

        if (FarmingMode)
        {
            if(fE != null)
                fE.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                
            GetComponent<Build>().enabled = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out mouseHit))
            {
                //fE = null; 
                if (mouseHit.transform.gameObject.tag == "field")
                {
                    fE = mouseHit.transform.parent.GetComponent<FieldElement>();
                    if (fE.planted)
                        fE.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Occupied;
                    else
                        fE.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = NotOccupied;
                }

                if (Input.GetMouseButtonDown(0) || clicked)
                {
                    if (!fE.planted)
                    {
                        var crop = Instantiate(plant, fE.transform.position, Quaternion.identity);
                        crop.GetComponent<Plant>().field = fE;
                        fE.connectedPlant = crop.gameObject;
                        fE.planted = true;
                    }
                    
                }
            }
        }
        else
        {
            GetComponent<Build>().enabled = true;
        }
        if (released)
        {
            plantIcons[currentPlant].SetActive(false);
            if (currentPlant == plants.Length - 1)
            {
                currentPlant = 0;
            }
            else
            {
                currentPlant++;
            }
            plantIcons[currentPlant].SetActive(true);
            plant = plants[currentPlant];
        }
    }

    public void ModeToggle()
    {
        if (FarmingMode)
        {
            FarmingMode = false;
            mode.text = "Build Mode";
        }
        else
        {
            FarmingMode = true;
            mode.text = "Farm Mode";
        }
    }
}
