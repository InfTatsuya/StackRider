using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlusText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amtText;

    public void Setup(int amt)
    {
        amtText.text = "+" + amt;
    }
}
