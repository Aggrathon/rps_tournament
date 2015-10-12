using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public InputField playerName;
	public Image fader;
	public float fadeInTime = 0.8f;
    public float fadeOutTime = 0.3f;

	IEnumerator Start()
	{
		Color c = Color.black;
		c.a = 1f;
		fader.color = c;
		fader.gameObject.SetActive(true);
		float time = 0f;
		if(playerName != null) //This is a temp fix for a bug in Unity 5.2
		{
			yield return null;
			Transform caretGO = playerName.transform.FindChild(playerName.transform.name + " Input Caret");
			(caretGO as RectTransform).pivot = new Vector2(0.5f, 0.3f);
			caretGO.GetComponent<CanvasRenderer>().SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
		}
		while(time< fadeInTime)
		{
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
			c.a = (fadeInTime - time) / fadeInTime;
			fader.color = c;
		}
		fader.gameObject.SetActive(false);
	}

	public void StartLadderTournament()
	{
		if(playerName.text != "")
		{
			TournamentState.instance.PlayerName = playerName.text;
		}
		StartCoroutine(ChangeScene("tournament"));
	}

	public void OpenMenu()
	{
		StartCoroutine(ChangeScene("menu"));
	}

	public void QuitApplication()
	{
		Application.Quit();
	}

	IEnumerator ChangeScene(string scene)
	{
		Color c = Color.black;
		c.a = 0f;
		fader.color = c;
		fader.gameObject.SetActive(true);
		float time = 0f;
		while (time < fadeOutTime)
		{
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
			c.a = time / fadeOutTime;
			fader.color = c;
		}
		Application.LoadLevel(scene);
	}
}
