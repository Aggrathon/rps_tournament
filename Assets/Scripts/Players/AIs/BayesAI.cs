using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BayesAI : ARPSPlayer {

    private List<Match.RPS> opponentHistory;

    public override void startMatch(Match match)
    {
        var opp = this == match.playerOne ? match.playerTwo : match.playerOne;
        memorizeOpponentHistory(opp);
        base.startMatch(match);
    }

    public void memorizeOpponentHistory(IRPSPlayer opponent)
    {
        var store = TournamentState.instance.Matches;
        var oppChoices = store.Select(m =>
        {
            return opponent == m.playerOne
                ? m.playerOneChoices as IEnumerable<Match.RPS>
                : m.playerTwoChoices as IEnumerable<Match.RPS>;
        }).Aggregate((a, b) => a.Concat(new[] { Match.RPS.UNDEFINED }).Concat(b)).ToList();
        opponentHistory = oppChoices;
    }

    public override void newRound()
    {
        var last = currentMatch.getLastChoiceOther(this);
        opponentHistory.Add(last);

        Match.RPS previous = Match.RPS.UNDEFINED;
        int[] startMove = new int[3];
        int[] afterRock = new int[3];    
        int[] afterPaper = new int[3];
        int[] afterScissors = new int[3];

        foreach (var choice in opponentHistory) {
            switch (previous)
            {
                case Match.RPS.Rock:
                    if (choice == Match.RPS.Rock)
                        afterRock[0] += 1;
                    else if (choice == Match.RPS.Paper)
                        afterRock[1] += 1;
                    else if (choice == Match.RPS.Scissors)
                        afterRock[2] += 1;
                    break;
                case Match.RPS.Paper:
                    if (choice == Match.RPS.Rock)
                        afterPaper[0] += 1;
                    else if (choice == Match.RPS.Paper)
                        afterPaper[1] += 1;
                    else if (choice == Match.RPS.Scissors)
                        afterPaper[2] += 1;
                    break;
                case Match.RPS.Scissors:
                    if (choice == Match.RPS.Rock)
                        afterScissors[0] += 1;
                    else if (choice == Match.RPS.Paper)
                        afterScissors[1] += 1;
                    else if (choice == Match.RPS.Scissors)
                        afterScissors[2] += 1;
                    break;
                case Match.RPS.UNDEFINED:
                    if (choice == Match.RPS.Rock)
                        startMove[0] += 1;
                    else if (choice == Match.RPS.Paper)
                        startMove[1] += 1;
                    else if (choice == Match.RPS.Scissors)
                        startMove[2] += 1;
                    break;
            }
            previous = choice;
        }

        int[] relevant;
        if (last == Match.RPS.Paper) relevant = afterPaper;
        else if (last == Match.RPS.Rock) relevant = afterRock;
        else if (last == Match.RPS.Scissors) relevant = afterScissors;
        else relevant = startMove;

        var total = relevant[0] + relevant[1] + relevant[2];
        var random = new System.Random().NextDouble() * total;

        if (random <= relevant[0])
        {
            setChoice(Match.RPS.Rock);
            return;
        }
        else random -= relevant[0];

        if (random <= relevant[1])
        {
            setChoice(Match.RPS.Paper);
            return;
        }
        else random -= relevant[1];

        setChoice(Match.RPS.Scissors);
    }

    private void setChoice(Match.RPS choice)
    {
        currentMatch.setPlayerChoice(this, choice);
    }

    public override void endMatch()
    {
        base.endMatch();
    }
}
