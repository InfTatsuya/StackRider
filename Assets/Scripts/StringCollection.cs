using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringCollection 
{
    //Animation
    public static int spinAim = Animator.StringToHash("spin");
    public static int runAnim = Animator.StringToHash("start");
    public static int winAnim = Animator.StringToHash("win");
    public static int loseAnim = Animator.StringToHash("lose");
    public static int resetAnim = Animator.StringToHash("win");
    public static int coinAnim = Animator.StringToHash("coin");

    //Tags
    public static string playerTag = "Player";
    public static string ballTag = "ball";
    public static string coinTag = "coin";
    public static string wallTag = "wall";
    public static string lavaTag = "lava";
    public static string desinationTag = "destination";
    public static string stepUpTag = "stepUp";
    public static string stepDownTag = "stepDown";
}
