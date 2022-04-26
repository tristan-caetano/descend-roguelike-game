using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text textBox;
    public AudioClip typingClip;
    public AudioSourceGroup audioSourceGroup;

    public GameObject playDialogue1;
    public Button playDialogue2Button;
    public Button playDialogue3Button;

    [TextArea]
    public string dialogue1;
    [TextArea]
    public string dialogue2;
    [TextArea]
    public string dialogue3;
    [TextArea]
    public string dialogue4;
    [TextArea]
    public string dialogue5;
    [TextArea]
    public string dialogue6;
    [TextArea]
    public string dialogue7;
    [TextArea]
    public string dialogue8;
    [TextArea]
    public string dialogue9;
    [TextArea]
    public string dialogue10;
    [TextArea]
    public string dialogue11;
    [TextArea]
    public string dialogue12;
    [TextArea]
    public string dialogue13;
    [TextArea]
    public string dialogue14;
    [TextArea]
    public string dialogue15;

    private DialogueVertexAnimator dialogueVertexAnimator;
    void Awake() {
        dialogueVertexAnimator = new DialogueVertexAnimator(textBox, audioSourceGroup);
        playDialogue2Button.onClick.AddListener(delegate { PlayDialogue10(); });
        playDialogue3Button.onClick.AddListener(delegate { PlayDialogue8(); });
    }

    public void PlayDialogue1() {
        PlayDialogue(dialogue1);
    }

    public void PlayDialogue2() {
        PlayDialogue(dialogue2);
    }

    public void PlayDialogue3() {
        PlayDialogue(dialogue3);
    }

    public void PlayDialogue4() {
        PlayDialogue(dialogue4);
    }

    public void PlayDialogue5() {
        PlayDialogue(dialogue5);
    }

    public void PlayDialogue6() {
        PlayDialogue(dialogue6);
    }

    public void PlayDialogue7() {
        PlayDialogue(dialogue7);
    }

    public void PlayDialogue8() {
        PlayDialogue(dialogue8);
    }

    public void PlayDialogue9() {
        PlayDialogue(dialogue9);
    }

    public void PlayDialogue10() {
        PlayDialogue(dialogue10);
    }

    public void PlayDialogue11() {
        PlayDialogue(dialogue11);
    }

    public void PlayDialogue12() {
        PlayDialogue(dialogue12);
    }

    public void PlayDialogue13() {
        PlayDialogue(dialogue13);
    }

    public void PlayDialogue14() {
        PlayDialogue(dialogue14);
    }

    public void PlayDialogue15() {
        PlayDialogue(dialogue15);
    }

    public void RemoveDialogue() {
        PlayDialogue(" ");
    }

    private Coroutine typeRoutine = null;
    public void PlayDialogue(string message) {
        this.EnsureCoroutineStopped(ref typeRoutine);
        dialogueVertexAnimator.textAnimating = true;
        List<DialogueCommand> commands = DialogueUtility.ProcessInputString(message, out string totalTextMessage);
        typeRoutine = StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, typingClip, null));
    }
}
