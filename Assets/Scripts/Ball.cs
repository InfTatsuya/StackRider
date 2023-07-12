using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] List<Material> materials = new List<Material>();
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Animator anim;
    [SerializeField] SphereCollider sphereCollider;

    private int materialIndex;
    public int MaterialIndex => materialIndex;

    private void Start()
    {
        materialIndex = Random.Range(0, materials.Count);
        meshRenderer.material = materials[materialIndex];
    }

    public void OnBallCollected()
    {
        sphereCollider.enabled = false;
        TriggerSpinAnim(true);
    }

    public void TriggerSpinAnim(bool isSpin)
    {
        anim.SetBool(StringCollection.spinAim, isSpin);
    }
}
