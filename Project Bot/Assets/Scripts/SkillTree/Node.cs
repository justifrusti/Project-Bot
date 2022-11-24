using ClipperLib;
using System;
using System.Text;
using UnityEditor;
using UnityEngine;

public class Node
{
    public Rect rect;

    public string title;

    public bool isDragged;
    public bool isSelected;

    public Rect rectID;
    public Rect rectUnlockLabel;
    public Rect rectUnlocked;
    public Rect rectCostLabel;
    public Rect rectCost;

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;
    public GUIStyle styleID;
    public GUIStyle styleField;

    public Action<Node> onRemoveNode;

    public Skill skill;

    private bool unlocked = false;

    private StringBuilder nodeTitle;

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, int id, bool unlocked, int cost, int[] dependencies)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);

        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);

        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        onRemoveNode = OnClickRemoveNode;

        float rowHeight = height / 7;

        rectID = new Rect(position.x, position.y + rowHeight, width, rowHeight);

        styleID = new GUIStyle();
        styleID.alignment = TextAnchor.UpperCenter;

        rectUnlocked = new Rect(position.x + width / 2, position.y + 3 * rowHeight, width / 2, rowHeight);

        rectUnlockLabel = new Rect(position.x, position.y + 3 * rowHeight, width / 2, rowHeight);

        styleField = new GUIStyle();
        styleField.alignment = TextAnchor.UpperRight;

        rectCostLabel = new Rect(position.x, position.y + 4 * rowHeight, width / 2, rowHeight);

        rectCost = new Rect(position.x + width / 2, position.y + 4 * rowHeight, 20, rowHeight);

        this.unlocked = unlocked;

        skill = new Skill();

        skill.id_Skill = id;
        skill.unlocked = unlocked;
        skill.cost = cost;
        skill.skill_Dependencies = dependencies;

        nodeTitle = new StringBuilder();

        nodeTitle.Append("ID: ");
        nodeTitle.Append(id);

    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
        rectID.position += delta;
        rectUnlocked.position += delta;
        rectUnlockLabel.position += delta;
        rectCost.position += delta;
        rectCostLabel.position += delta;
    }

    public void MoveTo(Vector2 pos)
    {
        rect.position = pos;
        rectID.position = pos;
        rectUnlocked.position = pos;
        rectUnlockLabel.position = pos;
        rectCost.position = pos;
        rectCostLabel.position = pos;
    }

    public void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();

        GUI.Box(rect, title, style);

        GUI.Label(rectID, nodeTitle.ToString(), styleID);

        GUI.Label(rectUnlockLabel, "Unlocked: ", styleField);

        if (GUI.Toggle(rectUnlocked, unlocked, ""))
        {
            unlocked = true;
        }
        else
        {
            unlocked = false;
        }

        skill.unlocked = unlocked;

        GUI.Label(rectCostLabel, "Cost: ", styleField);
        skill.cost = int.Parse(GUI.TextField(rectCost, skill.cost.ToString()));
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;

                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;

                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();

        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);

        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (onRemoveNode != null)
        {
            onRemoveNode(this);
        }
    }
}
