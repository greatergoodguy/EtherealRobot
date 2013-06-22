using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class TimeRecorder : MonoBehaviour {
	
	private int maxHighScores = 10;
	string filePath = "Assets/Resources/TimeRecords/PlayerTimeRecords.txt";
	
	private float startTime;
	private bool hasStarted = false;
	private bool timerIsGoing = false;  // currently unneeded
	
	private float rawPlayerTime = 0;
	private int playerSecondsTime = 0;
	private string playerClockTime = "00:00";
	

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {	
		KeyboardTimeInput();
	}
	
	void OnGUI(){
		if(hasStarted){
			rawPlayerTime = Time.time - startTime;
			playerSecondsTime = (int) rawPlayerTime;
			playerClockTime = convertToClockTime(playerSecondsTime);
		}
			GUI.Label(new Rect(10, 28, 100, 20), "Time " + playerClockTime);
	}	
	
	void KeyboardTimeInput(){
		if(Input.GetKeyDown(KeyCode.L)){
			if(!hasStarted){
				startTime = Time.time;
				hasStarted = true;
				timerIsGoing = true;
			}
			else{
				timerIsGoing = !timerIsGoing;	
			}	
		}
		else if(Input.GetKeyDown(KeyCode.K)){
			hasStarted = false;
			
			UpdateTimeRecords();
		}
	}
	
	string convertToClockTime(int timeInSeconds){
		int minutes = timeInSeconds / 60; // ((int)rawPlayerTime) / 60;
		int seconds = timeInSeconds % 60; // ((int)rawPlayerTime) % 60;
		
		string secondsStr = "";
		if(seconds < 10)
			secondsStr += "0";
		secondsStr += seconds.ToString();
		
		string minutesStr = "";
		if(minutes < 10)
			minutesStr += "0";
		minutesStr += minutes.ToString();
		
		string clockTime = minutesStr + ":" + secondsStr;
		return clockTime;
	}
	
	void UpdateTimeRecords(){
		
		string newFile = "";	
		string[] file = File.ReadAllLines(filePath);
		int fileLength = file.Length;
		string playerScoreString = playerSecondsTime + "-" + playerClockTime + "\r\n";
		
		if(fileLength == 0){
			newFile += playerScoreString;
		}
		else{
			int lineCount = 0;
			bool addedPlayerScore = false;
			if (fileLength > maxHighScores)
				fileLength = maxHighScores;
			while(lineCount < fileLength ){
				if(!addedPlayerScore){
					int hyphenIndex = file[lineCount].IndexOf("-");
					string highScoreTimeStr = file[lineCount].Substring(0, hyphenIndex);
					//int highScoreTime = (int) highScoreTimeStr;
					int highScoreTime; // = int.Parse(highScoreTimeStr);
					int.TryParse(highScoreTimeStr, out highScoreTime);
					if(playerSecondsTime < highScoreTime){	
						newFile += playerScoreString;
						addedPlayerScore = true;
					}
				}
				
				if(addedPlayerScore && lineCount == (maxHighScores - 1)){
					// Do nothing and exit the loop
				}
				else{
					newFile += file[lineCount] + "\r\n"; // adds high score from old file
				}
				lineCount++;
				
				// Add player's time to the file by end if there the number of high scores is less than maxHighScores
				if(!addedPlayerScore && fileLength < maxHighScores && (lineCount == fileLength)){
					newFile += playerScoreString;
				}
			}
		}
		File.WriteAllText(filePath, newFile);
	}
}
