using UnityEngine;
using UnityEngine.EventSystems;

public class OnlyInOneScene : MonoBehaviour {

	public string sceneName;
	
	void Awake () {
		if(Application.loadedLevelName.Equals(sceneName))
		{
			gameObject.SetActive(true);
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		else
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}
}
