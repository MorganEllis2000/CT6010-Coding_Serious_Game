using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CompilerExample : MonoBehaviour {

	public InputField input;
	public Text inputText;
	private string inputString;
	public Text ErrorLog;
	public Button runButton;
	public string answer;
	public Button nextButton;
	public GameObject go_InfoPannel;
	private string debugString;

	void Start() {
		input.GetComponent<InputField>().lineType = InputField.LineType.MultiLineNewline;

	}

	private void Awake() {
		nextButton.gameObject.SetActive(false);
		go_InfoPannel.gameObject.SetActive(false);
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

		if(string.Compare(debugString, answer) == 0) {
			Debug.Log("Correct");
			nextButton.gameObject.SetActive(true);
		} else {
			Debug.Log("Incorrect");
		}

		Debug.Log(debugString);
	}
	public void Update() {

	}

	/// <summary>
	/// loads the scene based on the index assigned in the inspector
	/// </summary>
	public void LoadByIndex(int sceneIndex) {
		SceneManager.LoadScene(sceneIndex);
	}

	public void OpenUIInformationHelp() {
		go_InfoPannel.gameObject.SetActive(true);
	}
	public void CloseUIInformationHelp() {
		go_InfoPannel.gameObject.SetActive(false);
	}

	public void ExecuteCode() {
		inputString = inputText.text.ToString();
		debugString = "";
		ErrorLog.text = "";
		Debug.ClearDeveloperConsole();
		var assembly = Compile(@"
								using UnityEngine;

								public class Test {
									public static void Foo() {" +
									inputString +
									"}}");


		var method = assembly.GetType("Test").GetMethod("Foo");
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
/*
using UnityEngine;

public class Test {
	public static void Foo() {
		var col = new Color(Random.value, Random.value, Random.value);
		var r = GameObject.FindObjectOfType<MeshRenderer>();
		r.material.color = col;

		r.transform.position = Random.insideUnitSphere;
	}
}
*/

