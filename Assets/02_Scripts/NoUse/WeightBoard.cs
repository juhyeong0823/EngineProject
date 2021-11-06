using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeightBoard : MonoBehaviour
{

    [SerializeField] private GameObject textPrefab;
    [SerializeField] private float radius;
    [SerializeField] private float textCount;
    [SerializeField] private int textValue;

    List<Vector3> aa = new List<Vector3>();
    void Start()
    {
        float heading;
        for (float a = 0; a < 360; a += 360 / textCount)
        {
            heading = a * Mathf.Deg2Rad;
            aa.Add(new Vector3(Mathf.Cos(heading) * radius, Mathf.Sin(heading) * radius,transform.position.z));
        }

        foreach(Vector3 pos in aa)
        {
            if (textValue % 5 == 0 && textValue < textCount)
            {
                GameObject textObj = Instantiate(textPrefab, this.transform);
                textObj.transform.localPosition = pos;
                textObj.transform.eulerAngles = new Vector3(0, 0, (float)(textValue * (360f / textCount)) );

                Text weightText = textObj.GetComponentInChildren<Text>();
                weightText.text = textValue.ToString();

                //if (textValue % 10 == 0)
                //{
                    weightText.fontSize = 50;
                //    weightText.fontStyle = FontStyle.Bold;
                //}
            }
            textValue++;
        }
    }
}
