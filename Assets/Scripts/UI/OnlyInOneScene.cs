using UnityEngine;
using UnityEngine.EventSystems;

public class OnlyInOneScene : MonoBehaviour {

	public string sceneName;
	
	void Awake () {
		if(Application.loadedLevelName.Equals(sceneName))
		{
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}
}
