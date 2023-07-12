using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] List<Material> materialList;
    [SerializeField] MeshRenderer meshRenderer;

    public void ChangeBackground(int index)
    {
        index = index % materialList.Count;
        meshRenderer.material = materialList[index];
    }
}
