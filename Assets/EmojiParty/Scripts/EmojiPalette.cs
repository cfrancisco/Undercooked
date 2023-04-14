using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmojiParty
{
    [CreateAssetMenu(fileName = "Data", menuName = "Emoji Party/Emoji Palette", order = 1)]
    public class EmojiPalette : ScriptableObject
    {
        [SerializeField] List<EmojiMaterialByName> materials;

        public EmojiMaterialByName GetMaterialByName(string name) {
            EmojiMaterialByName result = null;
            foreach (var material in materials)
            {
                if (material.name.ToString() == name) {
                    result = material;
                    break;
                }
            }
            return result;
        }
    }

    [Serializable]
    public class EmojiMaterialByName {
        [SerializeField] public EmojiMaterialName name;
        [SerializeField] public Material material;
    }

    public enum EmojiMaterialName {
        Skin = 10,
        EyePupil = 20,
        EyeBall = 21,
        EyeBright = 22,
        Cloth = 30,
        Teeth = 40,
        Water = 50,
        Sky = 60,
        Blood = 70,
        Bright = 80,
        Money = 90,
        Hands = 100,
        LightMetal = 110,
        Purple = 120,
        Cloud = 130,
        CloudTransparent = 131,
        ColorBit1 = 141,
        ColorBit2 = 142,
        ColorBit3 = 143,
        ExplosionDark = 150,
        ExplosionLight = 151,
        ExplosionShade = 152,
        Leather = 160,
        LeatherDark = 161,
        Nausea = 170,
        Party1 = 181,
        Party2 = 182,
        Party3 = 183,
        DarkFrame = 200,
        DarkHair = 210
    }
}
