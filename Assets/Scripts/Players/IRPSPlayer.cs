
public interface IRPSPlayer {
    void startMatch(Match match);
    void endMatch();
	void newRound();
	int health { get; set; }
}
