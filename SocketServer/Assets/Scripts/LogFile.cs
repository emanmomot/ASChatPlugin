using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LogFile : MonoBehaviour {

	private const string logPath = "LogFile.txt";

	private StreamWriter logWriter;

	void Start() {
		logWriter = new System.IO.StreamWriter (logPath, true);
		logWriter.WriteLine ("\n\n" + System.DateTime.Now);
	}

	void OnEnable() {
		Application.logMessageReceived += HandleLog;
	}

	void OnDisable() {
		Application.logMessageReceived -= HandleLog;
	}

	void OnDestroy() {
		logWriter.Close ();
	}

	void HandleLog(string logString, string stackTrace, LogType type) {
		logWriter.WriteLine (logString);
	}
}
