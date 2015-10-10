using UnityEngine;
using UnityEngine.EventSystems;

public class OnlyOneEventSystem : MonoBehaviour {
	
	void Awake () {
		if(GameObject.FindObjectsOfType<EventSystem>().Length > 1)
		{
			Destroy(gameObject);
		}
	}
}
