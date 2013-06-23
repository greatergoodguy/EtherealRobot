using UnityEngine;
using System;
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
	private float roundedPlayerTime = 0.00f;	
	private string playerClockTime = "00:00.00";
	
	string HighScoreList;

	// Use this for initialization
	void Start () {
		HighScoreList = GetHighScores();
	}
	
	// Update is called once per frame
	void Update () {	
		KeyboardTimeInput();
	}
	
	void OnGUI(){
		if(hasStarted){
			rawPlayerTime = Time.time - startTime;
			roundedPlayerTime = (float) Math.Round(rawPlayerTime, 2); 
			playerClockTime = convertToClockTime(roundedPlayerTime);
		}
		GUI.Label(new Rect(10, 28, 100, 20), "Time " + playerClockTime);
		GUI.Label(new Rect(Screen.width / 2, Screen.height / 10, 100, Screen.height - Screen.height / 10),
			"High Scores\n" + HighScoreList);
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
			HighScoreList = GetHighScores();
		}
	}
	
	string convertToClockTime(float timeRounded){
	
		int timeInSeconds = (int)(Math.Floor(timeRounded));	
		float decimalHolder = (float)(Math.Round(timeRounded - (Math.Floor(timeRounded)),2));
		
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
	
		string decimalsStr = decimalHolder.ToString().Substring(1);
		while(decimalsStr.Length < 3){
			decimalsStr += "0";	
		}
		
		string clockTime = minutesStr + ":" + secondsStr + decimalsStr;
		return clockTime;
	}
	
	void UpdateTimeRecords(){
		
		string newFile = "";	
		string[] file = File.ReadAllLines(filePath);
		int fileLength = file.Length;
		string playerScoreString = roundedPlayerTime + "-" + playerClockTime + Environment.NewLine;
		
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
					float highScoreTime; // = int.Parse(highScoreTimeStr);
					float.TryParse(highScoreTimeStr, out highScoreTime);
					if(roundedPlayerTime < highScoreTime){	
						newFile += playerScoreString;
						addedPlayerScore = true;
					}
				}
				
				if(addedPlayerScore && lineCount == (maxHighScores - 1)){
					// Do nothing and exit the loop
				}
				else{
					newFile += file[lineCount] + Environment.NewLine; // adds high score from old file
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
	
	string GetHighScores(){
		string allHighScores = "";
		string[] scoreList = File.ReadAllLines(filePath);

		for(int i = 0; i < scoreList.Length; i++){
			int hyphenIndex = scoreList[i].IndexOf("-");		
			string highScoreStr = scoreList[i].Substring(hyphenIndex + 1);
			allHighScores += highScoreStr + Environment.NewLine;
		}
		
		return allHighScores;
	}
}
