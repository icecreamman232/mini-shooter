// Editor/ConditionNodeDrawer.cs
// Place this file inside an Editor/ folder.
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using Shinrai.Modifiers;

// Cache for condition node types - shared between drawers
public static class ConditionNodeTypeCache
{
    public static Type[] Types { get; }
    public static string[] TypeNames { get; }

    static ConditionNodeTypeCache()
    {
        Types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ConditionNode).IsAssignableFrom(t))
            .ToArray();

        TypeNames = Types
            .Select(t => t.Name.Replace("Node", ""))
            .ToArray();
    }

    public static void ShowAddMenu(SerializedProperty listProp)
    {
        GenericMenu menu = new GenericMenu();

        for (int i = 0; i < Types.Length; i++)
        {
            Type type = Types[i];
            string name = TypeNames[i];

            menu.AddItem(new GUIContent(name), false, () =>
            {
                listProp.serializedObject.Update();
                int index = listProp.arraySize;
                listProp.InsertArrayElementAtIndex(index);
                var newElement = listProp.GetArrayElementAtIndex(index);
                newElement.managedReferenceValue = Activator.CreateInstance(type);
                listProp.serializedObject.ApplyModifiedProperties();
            });
        }

        menu.ShowAsContext();
    }
}

[CustomPropertyDrawer(typeof(ModifierRecord))]
public sealed class ModifierRecordDrawer : PropertyDrawer
{
    private const float PreviewHeight = 36f;
    private const float ButtonHeight = 20f;
    private const float Spacing = 4f;

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(prop, label, true);

        if (prop.isExpanded)
        {
            height += ButtonHeight + Spacing; // Add condition button
            height += PreviewHeight + Spacing; // Preview box
        }

        return height;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(pos, label, prop);

        float defaultHeight = EditorGUI.GetPropertyHeight(prop, label, true);
        Rect propertyRect = new Rect(pos.x, pos.y, pos.width, defaultHeight);

        EditorGUI.PropertyField(propertyRect, prop, label, true);

        if (prop.isExpanded)
        {
            float yPos = pos.y + defaultHeight + Spacing;
            float indent = EditorGUI.indentLevel * 15f;

            var condProp = prop.FindPropertyRelative("Conditions");

            Rect buttonRect = new Rect(pos.x + indent, yPos, pos.width - indent, ButtonHeight);
            if (EditorGUI.DropdownButton(buttonRect, new GUIContent("+ Add Condition"), FocusType.Keyboard))
            {
                ConditionNodeTypeCache.ShowAddMenu(condProp);
            }
            yPos += ButtonHeight + Spacing;

            Rect previewRect = new Rect(pos.x + indent, yPos, pos.width - indent, PreviewHeight);
            EditorGUI.HelpBox(previewRect, GetPreview(condProp), MessageType.Info);
        }

        EditorGUI.EndProperty();
    }

    private string GetPreview(SerializedProperty condProp)
    {
        if (condProp == null || condProp.arraySize == 0)
            return "Preview: Always active";

        var parts = new List<string>();
        for (int i = 0; i < condProp.arraySize; i++)
        {
            var elem = condProp.GetArrayElementAtIndex(i);
            if (elem.managedReferenceValue is ConditionNode node)
                parts.Add(node.GetSummary());
        }

        if (parts.Count == 0)
            return "Preview: Always active";

        return "Preview: " + string.Join(" AND ", parts);
    }
}

[CustomPropertyDrawer(typeof(CompositeConditionNode))]
public sealed class CompositeConditionNodeDrawer : PropertyDrawer
{
    private const float ButtonHeight = 20f;
    private const float Spacing = 2f;

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight; // Foldout

        if (prop.isExpanded)
        {
            // Operator field
            var opProp = prop.FindPropertyRelative("@operator");
            if (opProp != null)
                height += EditorGUI.GetPropertyHeight(opProp, true) + Spacing;

            // Children list
            var childrenProp = prop.FindPropertyRelative("children");
            if (childrenProp != null)
                height += EditorGUI.GetPropertyHeight(childrenProp, true) + Spacing;

            // Add button
            height += ButtonHeight + Spacing;
        }

        return height;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(pos, label, prop);

        // Foldout
        Rect foldoutRect = new Rect(pos.x, pos.y, pos.width, EditorGUIUtility.singleLineHeight);
        prop.isExpanded = EditorGUI.Foldout(foldoutRect, prop.isExpanded, label, true);

        if (prop.isExpanded)
        {
            EditorGUI.indentLevel++;
            float yPos = pos.y + EditorGUIUtility.singleLineHeight + Spacing;

            // Operator field
            var opProp = prop.FindPropertyRelative("@operator");
            if (opProp != null)
            {
                float opHeight = EditorGUI.GetPropertyHeight(opProp, true);
                Rect opRect = new Rect(pos.x, yPos, pos.width, opHeight);
                EditorGUI.PropertyField(opRect, opProp, new GUIContent("Operator"));
                yPos += opHeight + Spacing;
            }

            // Children list
            var childrenProp = prop.FindPropertyRelative("children");
            if (childrenProp != null)
            {
                float childrenHeight = EditorGUI.GetPropertyHeight(childrenProp, true);
                Rect childrenRect = new Rect(pos.x, yPos, pos.width, childrenHeight);
                EditorGUI.PropertyField(childrenRect, childrenProp, new GUIContent("Children"), true);
                yPos += childrenHeight + Spacing;

                // Add child button
                float indent = EditorGUI.indentLevel * 15f;
                Rect buttonRect = new Rect(pos.x + indent, yPos, pos.width - indent, ButtonHeight);
                if (EditorGUI.DropdownButton(buttonRect, new GUIContent("+ Add Child Condition"), FocusType.Keyboard))
                {
                    ConditionNodeTypeCache.ShowAddMenu(childrenProp);
                }
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}
#endif