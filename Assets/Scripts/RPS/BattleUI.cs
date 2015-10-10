using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleUI : MonoBehaviour {

	private HumanPlayer human;
	private ARPSPlayer ai;
	private Match match;

	private bool open = false;
	private bool next = false;
	private bool close = false;
	private bool work = false;

	private bool firstTurn;

	public RectTransform battleScreen;

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
		battleScreen.gameObject.SetActive(false);
	}

	void Update()
	{
		if(work)
		{
			if (open)
			{
				showOnMain();
				open = false;
			}
			if (next)
			{
				roundOnMain();
				next = false;
			}
			if (close)
			{
				hideOnMain();
				close = false;
			}
			work = false;
		}
	}

	public void Show(HumanPlayer player)
	{
		human = player;
		match = player.getMatch();
		ai = match.getOtherPlayer(player) as ARPSPlayer;
		firstTurn = true;

		open = true;
		work = true;
	}
	private void showOnMain()
	{
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

		//Animate open
		battleScreen.gameObject.SetActive(true);
		battleScreen.GetComponent<Animator>().SetTrigger("Show");
	}


	public void NewRound()
	{
		next = true;
		work = true;
	}
	private void roundOnMain()
	{
		setHealths();

		if (firstTurn)
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
		close = true;
		work = true;
	}
	private void hideOnMain()
	{
		//Animate close
		battleScreen.GetComponent<Animator>().SetTrigger("Hide");

		human.setFinishedClosing();
		human = null;
		ai = null;
		match = null;
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
		for (i = 0; i < match.getHealthPlayer(human); i++)
		{
			GameObject go = humanHealthContainer.GetChild(i).gameObject;
			go.SetActive(true);
			go.GetComponent<Image>().sprite = fullHeart;
		}
		for (; i < human.health; i++)
		{
			GameObject go = humanHealthContainer.GetChild(i).gameObject;
			go.SetActive(true);
			go.GetComponent<Image>().sprite = emptyHeart;
		}
		for (; i < humanHealthContainer.childCount; i++)
		{
			humanHealthContainer.GetChild(i).gameObject.SetActive(false);
		}

		for (i = 0; i < match.getHealthPlayer(ai); i++)
		{
			GameObject go = aiHealthContainer.GetChild(i).gameObject;
			go.SetActive(true);
			go.GetComponent<Image>().sprite = fullHeart;
		}
		for (; i < ai.health; i++)
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
