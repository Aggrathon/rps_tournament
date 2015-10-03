using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour {

	private HumanPlayer human;
	private ARPSPlayer ai;
	private Match match;

	private bool firstTurn;
	

	[Header("Icons")]
	public Sprite fullHeart;
	public Sprite emptyHeart;
	[Space(5)]
	public Sprite rock;
	public Sprite paper;
	public Sprite scissors;

	[Header("Human")]
	public RectTransform humanHealthContainer;
	public RectTransform humanHistoryContainer;
	public Image humanLook;
	public Text humanName;
	[Header("AI")]
	public RectTransform aiHealthContainer;
	public RectTransform aiHistoryContainer;
	public Image aiLook;
	public Text aiName;

	void Start () {
		gameObject.SetActive(false);
	}

	public void Show(HumanPlayer player, ARPSPlayer opponent, Match match)
	{
		human = player;
		ai = opponent;
		this.match = match;
		humanLook.sprite = human.look;
		aiLook.sprite = ai.look;
		humanName.text = human.name;
		aiName.text = ai.name;
		setHealths();

		for (int i = 0; i < humanHistoryContainer.childCount; i++)
		{
			humanHistoryContainer.GetChild(i).gameObject.SetActive(false);
		}
		for (int i = 0; i < aiHistoryContainer.childCount; i++)
		{
			aiHistoryContainer.GetChild(i).gameObject.SetActive(false);
		}
		firstTurn = true;

		//Animate open
	}

	public void NewTurn()
	{
		setHealths();

		if(firstTurn)
		{
			firstTurn = false;
		}
		else
		{
			Transform his = humanHistoryContainer.GetChild(0);
			his.SetSiblingIndex(humanHistoryContainer.childCount - 1);
			his.GetComponent<Image>().sprite = getChoiceSprite(match.getLastChoicePlayer(human));
			his.gameObject.SetActive(true);

			his = aiHistoryContainer.GetChild(aiHistoryContainer.childCount - 1);
			his.SetSiblingIndex(0);
			his.GetComponent<Image>().sprite = getChoiceSprite(match.getLastChoicePlayer(ai));
			his.gameObject.SetActive(true);

			//Animate result
		}
	}

	public void Hide()
	{
		human = null;
		ai = null;
		match = null;

		//Animate close
	}

	public void selectRock()
	{
		if (human != null)
		{
			human.selectChoice(Match.RPS.Rock);
		}
	}
	public void selectPaper()
	{
		if (human != null)
		{
			human.selectChoice(Match.RPS.Paper);
		}
	}
	public void selectScissors()
	{
		if (human != null)
		{
			human.selectChoice(Match.RPS.Scissors);
		}
	}

	private void setHealths()
	{
		int i;
		for (i = 0; i < human.health; i++)
		{
			GameObject go = humanHealthContainer.GetChild(i).gameObject;
			go.SetActive(true);
			go.GetComponent<Image>().sprite = fullHeart;
		}
		for (; i < human.maxHealth; i++)
		{
			GameObject go = humanHealthContainer.GetChild(i).gameObject;
			go.SetActive(true);
			go.GetComponent<Image>().sprite = emptyHeart;
		}
		for (; i < humanHealthContainer.childCount; i++)
		{
			humanHealthContainer.GetChild(i).gameObject.SetActive(false);
		}

		for (i = 0; i < ai.health; i++)
		{
			GameObject go = aiHealthContainer.GetChild(i).gameObject;
			go.SetActive(true);
			go.GetComponent<Image>().sprite = fullHeart;
		}
		for (; i < ai.maxHealth; i++)
		{
			GameObject go = aiHealthContainer.GetChild(i).gameObject;
			go.SetActive(true);
			go.GetComponent<Image>().sprite = emptyHeart;
		}
		for (; i < aiHealthContainer.childCount; i++)
		{
			aiHealthContainer.GetChild(i).gameObject.SetActive(false);
		}
	}

	private Sprite getChoiceSprite(Match.RPS rps)
	{
		switch(rps)
		{
			case Match.RPS.Rock:
				return rock;
			case Match.RPS.Paper:
				return paper;
			case Match.RPS.Scissors:
				return scissors;
		}
		return null;
	}
}
