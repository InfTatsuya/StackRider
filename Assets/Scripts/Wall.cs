using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] List<GameObject> wallStepList;

    [SerializeField] int heighStep;
    [SerializeField] bool isPushPlayerUp;
    
    public int HeighStep => heighStep;
    public bool IsPushPlayerUp => isPushPlayerUp;   

    private void Start()
    {
        SetupWall();
    }

    private void SetupWall()
    {
        for(int i = 0; i < wallStepList.Count; i++)
        {
            if(i < heighStep)
            {
                wallStepList[i].SetActive(true);
            }
            else
            {
                wallStepList[i].SetActive(false);
            }
        }
    }
}
