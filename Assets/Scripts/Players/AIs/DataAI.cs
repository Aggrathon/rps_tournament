using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class DataAI : ARPSPlayer {

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
        }).Aggregate((a, b) => a.Concat(b)).ToList();
        opponentHistory = oppChoices;
    }

    public override void newRound()
    {
        var last = currentMatch.getLastChoiceOther(this);
        opponentHistory.Add(last);
        var scissors = opponentHistory.Where(a => a == Match.RPS.Scissors).Count();
        var rocks = opponentHistory.Where(a => a == Match.RPS.Rock).Count();
        var papers = opponentHistory.Where(a => a == Match.RPS.Paper).Count();
        var total = scissors + papers + rocks;
        var random = new System.Random().NextDouble() * total;

        if (random <= scissors) { 
            setChoice(Match.RPS.Scissors); 
            return; 
        } 
        else random -= scissors;

        if (random <= rocks)
        {
            setChoice(Match.RPS.Rock);
            return;
        }
        else random -= rocks;

        setChoice(Match.RPS.Paper);
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
