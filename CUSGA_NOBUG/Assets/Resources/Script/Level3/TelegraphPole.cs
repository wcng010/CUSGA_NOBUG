using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine.Playables;

public class TelegraphPole : Ohters<TelegraphPole>
{
    void Update()
    {
        inter.InteractionChat();
    }

}
