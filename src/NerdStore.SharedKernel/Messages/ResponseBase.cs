using System;

namespace NerdStore.SharedKernel.Messages
{
    public class ResponseBase
    {
        public DateTime timestamp { get; set; }

        public object data { get; set; }

        public ResponseBase(object Data)
        {
            timestamp = DateTime.Now;
            this.data = Data;
        }
    }
}