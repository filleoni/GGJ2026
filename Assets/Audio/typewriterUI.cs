// Script for having a typewriter effect for UI
// Prepared by Nick Hwang (https://www.youtube.com/nickhwang)
// Want to get creative? Try a Unicode leading character(https://unicode-table.com/en/blocks/block-elements/)
// Copy Paste from page into Inpector

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class typewriterUI : MonoBehaviour
{
	TMP_Text _tmpProText;
	string writer;
	[SerializeField] Image _bgImage;
	
	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
	[SerializeField] string leadingChar = "";
	[SerializeField] bool leadingCharBeforeDelay = false;

	// Use this for initialization
	void Start()
	{
		_tmpProText = GetComponent<TMP_Text>()!;

		if (_tmpProText != null)
		{
			writer = _tmpProText.text;
			_tmpProText.text = "";

			StartCoroutine("TypeWriterTMP");
		}
	}

	IEnumerator TypeWriterTMP()
    {
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
			if (_tmpProText.text.Length > 0)
			{
				_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
			}
			_tmpProText.text += c;
			_tmpProText.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		StartCoroutine(FishEyeFade());
		
		if (leadingChar != "")
		{
			_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
		}
	}

	IEnumerator FishEyeFade()
	{
		while (_tmpProText.color.a > 0)
		{
			_tmpProText.color = _tmpProText.color - new Color(0, 0, 0, Time.deltaTime);
			_bgImage.color = _bgImage.color - new Color(0, 0, 0, Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}
	
}