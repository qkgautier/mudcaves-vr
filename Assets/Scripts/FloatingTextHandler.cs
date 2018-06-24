using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextHandler : MonoBehaviour {

    public Animator animator;
    public Text textUI;

    private float timer;
    private bool isPlaying = false;

    void Awake()
    {
        textUI = transform.Find("FloatingTextParent").Find("FloatingText").GetComponent<Text>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if(isPlaying && timer <= 0)
        {
            isPlaying = false;
            gameObject.SetActive(false);
        }
    }

    public void ShowText(string text, Color color, bool up = true, int fontSize = 30, float animationSpeed = 1f)
    {
        textUI.color = color;
        textUI.text = text;
        textUI.fontSize = fontSize;
        animator.speed = animationSpeed;
        if (up) { animator.Play("FloatingTextAnim", -1); }
        else { animator.Play("FloatingTextDownAnim", -1); }
        timer = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / animationSpeed;
        isPlaying = true;
    }
}
