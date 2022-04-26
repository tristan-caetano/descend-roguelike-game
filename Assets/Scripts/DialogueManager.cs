using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text textBox;
    public AudioClip typingClip;
    public AudioSourceGroup audioSourceGroup;

    public Button playDialogue1Button;
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
        playDialogue1Button.onClick.AddListener(delegate { PlayDialogue6(); });
        playDialogue2Button.onClick.AddListener(delegate { PlayDialogue10(); });
        playDialogue3Button.onClick.AddListener(delegate { PlayDialogue8(); });
    }

    private void PlayDialogue1() {
        PlayDialogue(dialogue1);
    }

    private void PlayDialogue2() {
        PlayDialogue(dialogue2);
    }

    private void PlayDialogue3() {
        PlayDialogue(dialogue3);
    }

    private void PlayDialogue4() {
        PlayDialogue(dialogue4);
    }

    private void PlayDialogue5() {
        PlayDialogue(dialogue5);
    }

    private void PlayDialogue6() {
        PlayDialogue(dialogue6);
    }

    private void PlayDialogue7() {
        PlayDialogue(dialogue7);
    }

    private void PlayDialogue8() {
        PlayDialogue(dialogue8);
    }

    private void PlayDialogue9() {
        PlayDialogue(dialogue9);
    }

    private void PlayDialogue10() {
        PlayDialogue(dialogue10);
    }

    private void PlayDialogue11() {
        PlayDialogue(dialogue11);
    }

    private void PlayDialogue12() {
        PlayDialogue(dialogue12);
    }

    private void PlayDialogue13() {
        PlayDialogue(dialogue13);
    }

    private void PlayDialogue14() {
        PlayDialogue(dialogue14);
    }

    private void PlayDialogue15() {
        PlayDialogue(dialogue15);
    }

    private Coroutine typeRoutine = null;
    void PlayDialogue(string message) {
        this.EnsureCoroutineStopped(ref typeRoutine);
        dialogueVertexAnimator.textAnimating = false;
        List<DialogueCommand> commands = DialogueUtility.ProcessInputString(message, out string totalTextMessage);
        typeRoutine = StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, typingClip, null));
    }
}
