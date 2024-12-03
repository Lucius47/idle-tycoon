/*
 * ReferencesFinder: A Remote Desktop Protocol Implementation
 *
 * Copyright 2011-2023 Muneeb Ullah <https://muneebullah.com/>
 * https://github.com/Lucius47
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains a context menu function to find scene references of selected gameObject and its components.
/// </summary>
public class ReferencesFinder : /*MonoBehaviour,*/ EditorWindow
{
    /// <summary>
    /// Finds scene references of selected gameObject and its components.
    /// </summary>
    [MenuItem("GameObject/Check References", false, 1)]
    public static void Find()
    {
        List<GameObject> rootObjects = new List<GameObject>();
        
        if (PrefabStageUtility.GetCurrentPrefabStage())
        {
            rootObjects.AddRange(PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.GetComponentsInChildren<Transform>(true).Select(t => t.gameObject));
        }
        else
        {
            // get root objects in scene
            Scene scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);
        }


        // get all components of selected gameObject, including built-in components
        object[] componentsOnSelectedObj = Selection.activeGameObject.GetComponents<Component>();

        // Keep a list of all the components that have references to the selected object
        targetMonoBehaviors = new();

        // iterate through all root objects
        for (int i = 0; i < rootObjects.Count; ++i)
        {
            GameObject gameObject = rootObjects[i];

            var components = gameObject.GetComponentsInChildren<MonoBehaviour>();

            foreach (var component in components)
            {
                // get all public and private fields of component
                var allFields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (FieldInfo FI in allFields)
                {
                    var field = FI.GetValue(component);

                    // Debug.LogError("RFM field: " + field, (GameObject)field);
                    // Debug.LogError("RFM activeGameObject: " + Selection.activeGameObject, (GameObject)Selection.activeGameObject);

                    if (componentsOnSelectedObj.Contains(field) || field == Selection.activeGameObject)
                    {
                        //Debug.LogError($"Reference found on GameObject: {gameObject.name}, Component: {component}," +
                        //    $" field: {field}");
                        if (!targetMonoBehaviors.ContainsKey(component))
                        {
                            targetMonoBehaviors.Add(component, FI);
                        }
                    }
                    // else(object)
                    // {
                    //     Debug.LogError("No references found");
                    // }
                }
            }
        }

        if (targetMonoBehaviors.Count > 0)
        {
            GetWindow<ReferencesFinder>("Hierarchy Highlighter");
        }
        else
        {
            EditorUtility.DisplayDialog("References Finder", "No references found", "OK");
            //Debug.LogError("No references found");
        }
    }

    private Vector2 scrollPosition;
    public static Dictionary<MonoBehaviour, FieldInfo> targetMonoBehaviors;

    private void OnGUI()
    {
        GUILayout.Label("Found References in Scene", EditorStyles.boldLabel);
        // begin layout group


        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var item in targetMonoBehaviors)
        {
            if (GUILayout.Button(item.Key.ToString() + " " + item.Value.Name))
            {
                EditorGUIUtility.PingObject(item.Key);

                Selection.activeObject = item.Key;
            }
        }

        GUILayout.EndScrollView();
    }
}