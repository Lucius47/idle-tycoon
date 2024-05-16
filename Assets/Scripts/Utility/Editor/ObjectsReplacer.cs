using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectsReplacer : EditorWindow
{
    public GameObject replacement;

    [MenuItem("GameObject/Objects Replacer", false, 1)]
    public static void Find()
    {
        GetWindow<ObjectsReplacer>("Objects Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Objects Replacer", EditorStyles.boldLabel);

        replacement = (GameObject)EditorGUILayout.ObjectField("Replacement", replacement, typeof(GameObject), true) as GameObject;


        if (GUILayout.Button("Replace"))
        {
            if (replacement == null)
            {
                Debug.LogError("No replacement object selected");
                return;
            }

            var selected = Selection.gameObjects;

            if (selected.Length == 0)
            {
                Debug.LogError("No objects selected");
                return;
            }

            foreach (var item in selected)
            {
                var newObject = Instantiate(replacement, item.transform.position, item.transform.rotation);
                newObject.transform.parent = item.transform.parent;
                newObject.transform.SetSiblingIndex(item.transform.GetSiblingIndex());
                newObject.transform.localScale = item.transform.localScale;
                newObject.name = item.name;

                // transfer the components
                foreach (var component in item.GetComponents<Component>())
                {
                    if (component is Transform) continue;
                    UnityEditorInternal.ComponentUtility.CopyComponent(component);

                    if (newObject.TryGetComponent(component.GetType(), out var existingComponent))
                    {
                        UnityEditorInternal.ComponentUtility.PasteComponentValues(existingComponent);
                    }
                    else
                    {
                        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(newObject);
                    }
                }

                //// transfer the children
                //foreach (Transform child in item.transform)
                //{
                //    child.parent = newObject.transform;
                //}

                DestroyImmediate(item);

                // Undo.RegisterCompleteObjectUndo(newObject, "Replace " + item.name + " with " + replacement.name);
            }
        }
    }
}
