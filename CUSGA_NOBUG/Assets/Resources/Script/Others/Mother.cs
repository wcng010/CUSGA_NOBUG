using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother : Ohters<Mother>
{
    void Update()
    {
        inter.InteractionChat();
    }
}
