
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RequiredInterfaceAttribute))]
public class InterfaceTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RequiredInterfaceAttribute attr = attribute as RequiredInterfaceAttribute;

        if (property.propertyType != SerializedPropertyType.ObjectReference)
        {
            EditorGUI.LabelField(position, label.text, "RequiredInterface Attribute can only be used with MonoBehaviour Components!");
            return;
        }

        MonoBehaviour oldComp = property.objectReferenceValue as MonoBehaviour;

        GameObject temp = null;
        string oldName = "";

        if (Event.current.type == EventType.Repaint)
        {
            if (oldComp == null)
            {
                temp = new GameObject("None [" + attr.requiredType.Name + "]");
                oldComp = temp.AddComponent<MonoInterface>();
            }
            else
            {
                oldName = oldComp.name;
                oldComp.name = oldName + " [" + attr.requiredType.Name + "]";
            }
        }

        MonoBehaviour comp = EditorGUI.ObjectField(position, label, oldComp, typeof(MonoBehaviour), true) as MonoBehaviour;

        if (Event.current.type == EventType.Repaint)
        {
            if (temp != null)
                GameObject.DestroyImmediate(temp);
            else
                oldComp.name = oldName;
        }

        if (oldComp == comp)
            return;

        if (comp != null)
        {
            if (comp.GetType() != attr.requiredType)
                comp = comp.gameObject.GetComponent(attr.requiredType) as MonoBehaviour;

            if (comp == null)
                return;
        }

        property.objectReferenceValue = comp;
        property.serializedObject.ApplyModifiedProperties();
    }
}

public class MonoInterface : MonoBehaviour
{
}
