using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintOpener : MonoBehaviour
{
	public GameObject goHintPanel;

	public void OpenPanel() {
		if(goHintPanel != null) {
			Animator animator = goHintPanel.GetComponent<Animator>();
			if (animator != null) {
				bool isOpen = animator.GetBool("Open");

				animator.SetBool("Open", !isOpen);
				goHintPanel.gameObject.transform.GetChild(0).gameObject.SetActive(!isOpen);
			}
		}
	}
}
