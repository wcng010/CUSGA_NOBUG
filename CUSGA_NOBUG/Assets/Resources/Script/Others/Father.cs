using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Father : Ohters<Father>
{
    void Update()
    {
        inter.InteractionChat();
    }
}
