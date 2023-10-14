using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Undercooked.Data;

namespace Undercooked.Model
{
    public class Request
    {
        public RequestData _requestData;
        public RequestType requestType;
        public GameObject _currentPickable;

        public void setCurrentPickable(GameObject cp)
        {
            _currentPickable = cp;
        }

        public void setRequestType(RequestType rt){
            _requestData = new RequestData();
            _requestData.type = rt;

            if (rt == RequestType.CutTomato)
            {
                _requestData.messageToAsk = this.getTomatoMessage();
            }

            
            if (rt == RequestType.CutOnion)
            {
                _requestData.messageToAsk = this.getOnionMessage();
            }

            
            if (rt == RequestType.DeliverOrder)
            {
                _requestData.messageToAsk = this.getDeliveryMessage();
            }

            
            if (rt == RequestType.NoOperation)
            {
                _requestData.messageToAsk =  this.getEmptyMessage();
            }

            this.requestType = rt;
        }

        private string[] getEmptyMessage(){
            return new string[] { ""};
        }


        private string[] getTomatoMessage(){
            return new string[] { "Por favor, você pode \n cortar o tomate?"};
        }

        private string[] getOnionMessage(){
            return new string[] { "Por favor, você pode \n cortar a cebola?"};
        }


        private string[] getDeliveryMessage(){
            return new string[] { "Por favor, você pode \n entregar o prato?"};
        }

    }
}