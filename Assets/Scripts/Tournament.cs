using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tournament : MonoBehaviour {

    public GameObject cell;
    GameObject canvas;
	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
        Create(16);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Create(int player_amount)
    {
        var next_y = -30 + player_amount / 2 * 64;
        for (var i = 1; i <= player_amount; i++)
        {
            var obj = (GameObject)Instantiate(cell, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(canvas.transform, false);
            obj.transform.GetComponentsInChildren<Text>()[1].text = i.ToString();
            obj.transform.position = new Vector3(0, next_y, 0);

            next_y -= 64;
        }
    }
}
