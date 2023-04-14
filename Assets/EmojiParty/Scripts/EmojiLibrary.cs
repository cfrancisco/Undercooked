using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmojiParty
{
    [CreateAssetMenu(fileName = "Data", menuName = "Emoji Party/Emoji Library", order = 1)]
    public class EmojiLibrary : ScriptableObject
    {
        [SerializeField] List<EmojiItem> _emojis;

        public GameObject GetModelByEmojiName(EmojiName emojiName) {
            var found = _emojis.Where(e => e.emojiName == emojiName).Select(e => e.emojiModel).ToList();
            if (found.Count > 0) {
                return found[0];
            } else {
                return null;
            }
        }
    }

    [Serializable]
    public class EmojiItem
    {
        [SerializeField] public EmojiName emojiName;
        [SerializeField] public GameObject emojiModel;

        public override string ToString() {
            return emojiName.ToString();
        }
    }

    public enum EmojiName {
        Emoji001,
        Emoji002,
        Emoji003,
        Emoji004,
        Emoji005,
        Emoji006,
        Emoji007,
        Emoji008,
        Emoji009,
        Emoji010,
        Emoji011,
        Emoji012,
        Emoji013,
        Emoji014,
        Emoji015,
        Emoji016,
        Emoji017,
        Emoji018,
        Emoji019,
        Emoji020,
        Emoji021,
        Emoji022,
        Emoji023,
        Emoji024,
        Emoji025,
        Emoji026,
        Emoji027,
        Emoji028,
        Emoji029,
        Emoji030,
        Emoji031,
        Emoji032,
        Emoji033,
        Emoji034,
        Emoji035,
        Emoji036,
        Emoji037,
        Emoji038,
        Emoji039,
        Emoji040,
        Emoji041,
        Emoji042,
        Emoji043,
        Emoji044,
        Emoji045,
        Emoji046,
        Emoji047,
        Emoji048,
        Emoji049,
        Emoji050,
        Emoji051,
        Emoji052,
        Emoji053,
        Emoji054,
        Emoji055,
        Emoji056,
        Emoji057,
        Emoji058,
        Emoji059,
        Emoji060,
        Emoji061,
        Emoji062,
        Emoji063,
        Emoji064,
        Emoji065,
        Emoji066,
        Emoji067,
        Emoji068,
        Emoji069,
        Emoji070,
        Emoji071,
        Emoji072,
        Emoji073,
        Emoji074,
        Emoji075,
        Emoji076,
        Emoji077,
        Emoji078,
        Emoji079,
        Emoji080,
        Emoji081,
        Emoji082,
        Emoji083,
        Emoji084,
        Emoji085,
        Emoji086,
        Emoji087,
        Emoji088,
        Emoji089,
        Emoji090,
        Emoji091,
        Emoji092,
        Emoji093,
        Emoji094,
        Emoji095,
        Emoji096,
        Emoji097,
        Emoji098,
        Emoji099,
        Emoji100,
        Emoji101,
        Emoji102,
        Emoji103,
        Emoji104,
        Emoji105,
        Emoji141,
        Emoji166,
        Emoji168,
        Emoji168A,
        Emoji170,
        Emoji170A,
        Emoji189,
        Emoji189A,
        Emoji191,
        Emoji191A,
        Emoji192,
        Emoji193,
        Emoji194,
        Emoji194A,
        Emoji195,
        Emoji203,
        Emoji1011,
    }
}
