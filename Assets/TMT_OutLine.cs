using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMT_OutLine : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public Color color = Color.white;

    private void Awake()
    {
        textMeshPro = this.GetComponent<TextMeshProUGUI>();
        textMeshPro.outlineWidth = 0.2f;
        textMeshPro.outlineColor = color;
        textMeshPro.text = "Å×½ºÆ®";
    }
}
