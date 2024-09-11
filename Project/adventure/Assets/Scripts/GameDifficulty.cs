// Static class
public static class GameSettings {

	public enum gameDifficulties {Easy, Normal, Hard,Explore};

	// static float variable which will hold saved data
	public static gameDifficulties difficulty;

	public static bool showIntroLevelMessage;

	// private constructor
	static GameSettings() {
		difficulty = gameDifficulties.Easy;
		showIntroLevelMessage = true;
	}
}