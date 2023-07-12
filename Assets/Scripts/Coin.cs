using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] GameObject model;
    [SerializeField] Animator anim;
    private void Update()
    {
        transform.Rotate(0f, 150f * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringCollection.playerTag))
        {
            anim.SetBool(StringCollection.coinAnim, true);

            Destroy(this.gameObject, 1f);
        }
    }
}
