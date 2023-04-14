#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EmojiParty
{
    [CustomEditor(typeof(Emoji))]
    public class EmojiEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            Emoji emoji = (Emoji)target;

            var meshButton = GUILayout.Button("Update Mesh");
            if (meshButton) {
                if (emoji.library == null) {
                    EditorUtility.DisplayDialog("Missing Library", "You must provide a library object", "Ok");
                    return;
                }

                emoji.UpdateMesh();

                EditorUtility.SetDirty(emoji.gameObject);
            }

            var materialsButton = GUILayout.Button("Update Materials");
            if (materialsButton) {
                if (emoji.palette == null) {
                    EditorUtility.DisplayDialog("Missing Palette", "You must provide a palette object", "Ok");
                    return;
                }

                emoji.UpdateMaterialsFromPalette();

                EditorUtility.SetDirty(emoji.gameObject);
            }
        }
    }
}

#endif