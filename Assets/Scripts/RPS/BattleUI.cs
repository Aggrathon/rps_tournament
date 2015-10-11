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
	public Animator humanHand;
	[Header("AI")]
	public RectTransform aiHealthContainer;
	public RectTransform aiHistoryContainer;
	public Image aiLook;
	public Text aiName;
	public Text aiTaunt;
	public Animator aiHand;

	[Header("Completion")]
	public RectTransform completionPanel;
	public Text completionResult;
	public Image completionImage;
	public Text completionPhrase;
	public Button completionClose;

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

		aiTaunt.text = ai.catchPhrase;
		aiTaunt.enabled = true;

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
			Match.RPS ch = match.getLastChoicePlayer(human);
			Match.RPS ca = match.getLastChoicePlayer(ai);
			Transform his = humanHistoryContainer.GetChild(0);
			his.SetSiblingIndex(humanHistoryContainer.childCount - 1);
			his.GetComponent<Image>().sprite = getChoiceSprite(ch);
			his.gameObject.SetActive(true);
			switch(ch)
			{
				case Match.RPS.Rock: humanHand.SetTrigger("Rock"); break;
				case Match.RPS.Scissors: humanHand.SetTrigger("Scissors"); break;
				case Match.RPS.Paper: humanHand.SetTrigger("Paper"); break;
			}

			his = aiHistoryContainer.GetChild(aiHistoryContainer.childCount - 1);
			his.SetSiblingIndex(0);
			his.GetComponent<Image>().sprite = getChoiceSprite(ca);
			his.gameObject.SetActive(true);
			switch (ca)
			{
				case Match.RPS.Rock: aiHand.SetTrigger("Rock"); break;
				case Match.RPS.Scissors: aiHand.SetTrigger("Scissors"); break;
				case Match.RPS.Paper: aiHand.SetTrigger("Paper"); break;
			}

			//Animate result
			IRPSPlayer winner = match.getLastWinner();
			if(winner != null)
			{
				if (winner == human)
				{
					aiLook.transform.parent.GetComponent<Animator>().SetTrigger("Flash");
				}
				else if (winner == ai)
				{
					humanLook.transform.parent.GetComponent<Animator>().SetTrigger("Flash");
				}
			}

			aiTaunt.enabled = false;
		}
	}

	public void Hide()
	{
		next = true;
		close = true;
		work = true;
	}
	private void hideOnMain()
	{
		//Animate close
		battleScreen.GetComponent<Animator>().SetTrigger("Hide");

		completionPanel.gameObject.SetActive(true);
		if(match.getLastWinner() == human)
		{
			completionPhrase.text = ai.loosePhrase;
			completionResult.text = "<color=green>You Won</color>";
        }
		else
		{
			completionPhrase.text = ai.winPhrase;
			completionResult.text = "<color=red>You Lost</color>";
		}
		completionImage.sprite = ai.look;
		completionClose.onClick.RemoveAllListeners();
		completionClose.onClick.AddListener(() =>
		{
			human.setFinishedClosing();
			human = null;
			ai = null;
			match = null;
			completionPanel.gameObject.SetActive(false);
		});
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
			default:
				return null;
		}
	}
}
