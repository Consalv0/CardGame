using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

class AssemblyCardBehaviours
{
    public static Assembly assembly {
        get { return Assembly.GetAssembly(typeof(CardBehaviour)); }
    }
}

[CustomEditor(typeof(CardInfo))]
public class CardInfoEditor : Editor
{
    public string[] classTypes;
    private int selectedType = 0;

    CardInfo instance {
        get { return (CardInfo)target; }
    }

    public void OnEnable()
    {
        List<System.Type> types = new List<System.Type>(AssemblyCardBehaviours.assembly.GetTypes());
        types.RemoveAll(x => !typeof(CardBehaviour).IsAssignableFrom(x) || x == typeof(CardBehaviour));
        classTypes = new string[types.Count];
        int count = 0;
        foreach (System.Type type in types)
        {
            classTypes[count] = type.AssemblyQualifiedName;
            count++;
        }
        selectedType = ArrayUtility.FindIndex<string>(classTypes, x => instance.cardBehaviourName == x);
    }

    public override void OnInspectorGUI()
    {
        selectedType = EditorGUILayout.Popup(new GUIContent("Card Behavior Type"), selectedType, classTypes);
        if (selectedType >= 0 && selectedType <= classTypes.Length)
        {
            instance.cardBehaviourName = classTypes[selectedType];
        }

        EditorGUILayout.Space();

        DrawDefaultInspector();
    }

}

