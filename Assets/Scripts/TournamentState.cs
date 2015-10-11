using System;
using System.Collections.Generic;

public class TournamentState {

    public List<ARPSPlayer> Competitors
    {
        get;
        set;
    }

    public List<Match> Matches
    {
        get;
        set;
    }

	public string PlayerName = "Player";

    public static TournamentState instance = new TournamentState();

    private TournamentState()
    {
        Competitors = new List<ARPSPlayer>();
        Matches = new List<Match>();
    }
}
