using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CompilerExample : MonoBehaviour {

	public InputField input;
	public Text inputText;
	private string inputString;
	public Text ErrorLog;
	public Button runButton;
	private string Answer;
	public Button nextButton;
	private string debugString;
	private string lastInputString;

	string[] subs;

	[TextArea(15, 20)]
	public string Description = "";
	[TextArea(15, 20)]
	public string codeAnswer = "";
	[TextArea(15, 20)]
	public string textAnswer = "";



	void Start() {
		int iCurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int levelAt = PlayerPrefs.GetInt("levelAt", 1);
		if (iCurrentSceneIndex < levelAt) {
			nextButton.gameObject.SetActive(true);
		} else {
			nextButton.gameObject.SetActive(false);
		}
		input.GetComponent<InputField>().lineType = InputField.LineType.MultiLineNewline;

		input.text = Description;

		subs = input.text.ToString().Split(' ', '\t');
		//lastInputString = inputString;

		/*
		for (int i = 0; i < subs.Length; i++) {
			if (subs[i].ToString() == "using" || subs[i].ToString() == "public" || subs[i].ToString() == "void") {
				string j = subs[i];
				Debug.Log(j);
				input.text = input.text.Replace(j, "<color=#0000ffff>" + j + "</color>");
				
			}
			if (subs[i].ToString() == "class" || subs[i].ToString() == "Test") {
				string j = subs[i];
				Debug.Log(j);
				input.text = input.text.Replace(j, "<color=#008000ff>" + j + "</color>");
			}
			if (subs[i].ToString() == "Debug") {
				string j = subs[i];
				Debug.Log(j);
				input.text = input.text.Replace(j, "<color=#ffa500ff>" + j + "</color>");
			}
		}*/
	}

	private void Awake() {
		//nextButton.gameObject.SetActive(false);
		Debug.ClearDeveloperConsole();
		//PlayerPrefs.DeleteAll();
	}

	void OnEnable() {
		Application.logMessageReceived += LogCallback;
	}

	void OnDisable() {
		Application.logMessageReceived -= LogCallback;
	}

	void LogCallback(string logString, string stackTrace, LogType type) {
		//ErrorLog.text = logString;
		//Or Append the log to the old one
		ErrorLog.text += logString + "\r\n";
		debugString += logString;

		if (textAnswer != null)
		{
			if (string.Compare(debugString, textAnswer) == 0)
			{
				Debug.Log("Correct");
				nextButton.gameObject.SetActive(true);
				int nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
				PlayerPrefs.SetInt("levelAt", nextSceneLoad);
			} else {
				Debug.Log("Incorrect");
			}
		} 
		
		if(string.Compare(inputString, codeAnswer) == 0) {
			Debug.Log("Correct");
			nextButton.gameObject.SetActive(true);
			int nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
			PlayerPrefs.SetInt("levelAt", nextSceneLoad);
		} else {
			Debug.Log("Incorrect");
		}

		//Debug.Log(debugString);
	}
	public void Update() {
		/*
		if (input.text.ToString() != inputString) {
			for (int i = 0; i < subs.Length; i++) {
				if (subs[i].ToString() == "using" || subs[i].ToString() == "public" || subs[i].ToString() == "void") {
					string j = subs[i];
					Debug.Log(j);
					input.text = input.text.Replace(j, "<color=#0000ffff>" + j + "</color>");
				} else if (subs[i].ToString() != "using" || subs[i].ToString() != "public" || subs[i].ToString() != "void") {
					string j = subs[i];
					Debug.Log(j);
					input.text = input.text.Replace("<color=#0000ffff>" + j + "</color>", j);
				}
				if (subs[i].ToString() == "class" || subs[i].ToString() == "Test") {
					string j = subs[i];
					Debug.Log(j);
					input.text = input.text.Replace(j, "<color=#008000ff>" + j + "</color>");
				}
				if (subs[i].ToString() == "Debug") {
					string j = subs[i];
					Debug.Log(j);
					input.text = input.text.Replace(j, "<color=#ffa500ff>" + j + "</color>");
				}
			}
			subs = input.text.ToString().Split(' ', '\t');
		
		}*/
	}

	public void ExecuteCode() {
		inputString = inputText.text.ToString();
		debugString = "";
		ErrorLog.text = "";
		Debug.ClearDeveloperConsole();
		var assembly = Compile(inputString);

		var method = assembly.GetType("Test").GetMethod("main");
		var del = (Action)Delegate.CreateDelegate(typeof(Action), method);
		del.Invoke();
	}

	public Assembly Compile(string source) {
		var provider = new CSharpCodeProvider();
		var param = new CompilerParameters();

		// Add ALL of the assembly references
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
			param.ReferencedAssemblies.Add(assembly.Location);
		}
		
		// Add specific assembly references
		//param.ReferencedAssemblies.Add("System.dll");
		//param.ReferencedAssemblies.Add("CSharp.dll");
		//param.ReferencedAssemblies.Add("UnityEngines.dll");

		// Generate a dll in memory
		param.GenerateExecutable = false;
		param.GenerateInMemory = true;

		// Compile the source
		var result = provider.CompileAssemblyFromSource(param, source);

		if (result.Errors.Count > 0) {
			var msg = new StringBuilder();
			foreach (CompilerError error in result.Errors) {
				msg.AppendFormat("Error ({0}): {1}\n",
					error.ErrorNumber, error.ErrorText);
					ErrorLog.text = msg.ToString();
			}
			throw new Exception(msg.ToString());

		}
	
		// Return the assembly
		return result.CompiledAssembly;
	}
}

