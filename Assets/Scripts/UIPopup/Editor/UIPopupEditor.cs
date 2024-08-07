using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(UIPopup))]
public class UIPopupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIPopup panel = (UIPopup)target;

        string newPrefix = "Panel_ID_" + panel.id.ToString("D2");
        string currentName = panel.gameObject.name;

        int prefixEndIndex = currentName.LastIndexOf('_');
        if (prefixEndIndex > 0)
        {
            currentName = currentName.Substring(prefixEndIndex + 1); 
        }

        if (!panel.gameObject.name.StartsWith(newPrefix))
        {
            panel.gameObject.name = newPrefix + "_" + currentName;
        }


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Information", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Highest ID in Scene:", UIPopupManager.GetHighestPanelId().ToString());

        List<UIPopup> duplicates = UIPopupManager.FindPanelsWithSameId(panel.id);
        bool hasDuplicates = duplicates.Count > 1;
        GUIStyle duplicateStyle = new GUIStyle(EditorStyles.label);
        duplicateStyle.normal.textColor = hasDuplicates ? Color.red : Color.green;
        duplicateStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.LabelField("Has Duplicate IDs?:", hasDuplicates ? "Yes" : "No", duplicateStyle);

        if (hasDuplicates)
        {
            EditorGUILayout.LabelField("Duplicate Panels:");
            foreach (var dupPanel in duplicates)
            {
                if (dupPanel != panel)
                {
                    EditorGUILayout.LabelField($"Panel Name: {dupPanel.name}, ID: {dupPanel.id}");
                }
            }
        }
    }
}
