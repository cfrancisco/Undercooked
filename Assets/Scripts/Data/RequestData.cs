using Undercooked.Model;
using UnityEngine;

namespace Undercooked.Data
{
    public class RequestData
    {
        [Header("Generals")]
        public RequestType type;
        public string[] messageToAsk;
    }
}