using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameObject[] stages = new GameObject[3];
    public FieldElement field;
    public GameObject produce;
    bool stageComplete = false;
    int currentStage;
    
    private void Awake()
    {
        currentStage = 0;
        stages[0].SetActive(true);
        for(int i=1; i<stages.Length; i++)
        {
            stages[i].SetActive(false);
        }
        StartCoroutine(timer(8));
    }

    private void Update()
    {
        if (stageComplete)
        {
            if (currentStage == stages.Length - 1)
            {
                var crop = Instantiate(produce, field.transform.position, Quaternion.identity);
                crop.GetComponent<Crop>().field = field;
                Destroy(this.gameObject);
            }
            else
            {
                stages[currentStage].SetActive(false);
                currentStage++;
                stages[currentStage].SetActive(true);
                stageComplete = false;
                StartCoroutine(timer(8));
            }
            
        }
        
    }

    IEnumerator timer(float f)
    {
        yield return new WaitForSeconds(f);
        stageComplete = true;
    }
}
