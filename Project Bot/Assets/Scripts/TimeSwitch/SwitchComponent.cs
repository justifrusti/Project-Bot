using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchComponent : MonoBehaviour
{
    public enum ComponentType
    {
        Door,
        Platform
    }

    public enum Action
    {
        Disable,
        Enable
    }

    public ComponentType type;
    public Action currentAction;
}
