using Undercooked.Model;

namespace Undercooked.Requests
{
    public class RequestMade
    {
        public int _timestampStart;
        public int _timestampEnd;
        public ResponseType _faceShown;
        public RequestType _actionRealized; 

        public RequestMade(int timestampStart, int timestampEnd,ResponseType faceShown, RequestType actionRealized){
            this._timestampStart = timestampStart;
            this._timestampEnd = timestampEnd;
            this._faceShown = faceShown;
            this._actionRealized = actionRealized; 
        }
    }
}