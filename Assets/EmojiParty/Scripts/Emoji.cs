using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmojiParty
{
    public class Emoji : MonoBehaviour
    {
        
        [SerializeField] EmojiName _emoji;

        [SerializeField] EmojiLibrary _library;
        public EmojiLibrary library => _library;

        [SerializeField] EmojiPalette _palette;
        public EmojiPalette palette => _palette;

        [SerializeField] bool _rotatedBack = true;

        Quaternion quaternionForRotatedBack = Quaternion.Euler(0, 180, 0);

        public override string ToString() {
            return _emoji.ToString();
        }

        public void UpdateMesh() {
            var oldModel = GetComponentInChildren<MeshFilter>();
            if (oldModel != null) {
                oldModel.transform.parent = null;
                GameObject.DestroyImmediate(oldModel.gameObject);
            }

            var modelByName = _library.GetModelByEmojiName(_emoji);
            if (modelByName == null) {
                return;
            }

            GameObject newModel = UnityEngine.Object.Instantiate(modelByName);
            newModel.transform.position = gameObject.transform.position;
            newModel.transform.parent = gameObject.transform;

            if (_rotatedBack) {
                newModel.transform.rotation = quaternionForRotatedBack;
            }

            UpdateMaterialsFromPalette();
        }

        public void UpdateMaterialsFromPalette() {
            if (palette == null) {
                throw new Exception("No palette has been assigned in this emoji object.");
            }

            var meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                var materials = meshRenderer.sharedMaterials;

                for (int iMat=0; iMat<meshRenderer.sharedMaterials.Length; iMat++)
                {
                    Material mat = meshRenderer.sharedMaterials[iMat];
                    string matName = CleanMaterialName(mat.name);
                    EmojiMaterialByName matByName = palette.GetMaterialByName(matName);
                    if (matByName == null) {
                        continue;
                    }
                    materials[iMat] = matByName.material;
                }

                meshRenderer.materials = materials;
            }
        }

        string CleanMaterialName(string name) {
            return name.Replace(" (Instance)", "").Replace(" ", "");
        }
    }
}
