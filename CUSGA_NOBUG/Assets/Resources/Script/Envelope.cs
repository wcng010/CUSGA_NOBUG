using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Envelope : MonoBehaviour
{
    Animator animator;
    public Button butSwitch;

    public GameObject EnveOpen;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenEnvelope()
    {
        EnveOpen.SetActive(true);
    }

    public void CloseEnvelope()
    {
        EnveOpen.SetActive(false);
    }

    public void butSwitchDown()
    {
        animator.enabled = true;
        butSwitch.enabled = false;
    }

    public void Insert()
    {
        transform.SetAsFirstSibling();
    }

    public void SwitchIsOK()
    {
        animator.enabled = false;
        butSwitch.enabled = true;
    }
}
