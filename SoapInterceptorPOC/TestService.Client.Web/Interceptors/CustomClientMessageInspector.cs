using System;
using System.ServiceModel.Dispatcher;
using System.IO;
using System.ServiceModel;
using System.Xml;

namespace TestService.Client.Web.Interceptors
{


    
    public class CustomClientMessageInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            // Create a buffered copy of the reply message
            var buffer = reply.CreateBufferedCopy(int.MaxValue);
            var copy = buffer.CreateMessage();
            reply = buffer.CreateMessage(); // Restore the original message

            // Log the incoming response
            LogMessage(copy, "C:\\temp\\IncomingResponse.log");
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            // Create a buffered copy of the request message
            var buffer = request.CreateBufferedCopy(int.MaxValue);
            var copy = buffer.CreateMessage();
            request = buffer.CreateMessage(); // Restore the original message

            // Log the outgoing request
            LogMessage(copy, "C:\\temp\\OutgoingRequest.log");
            return null;
        }


        private void LogMessage(System.ServiceModel.Channels.Message message, string fileName)
        {
            using (var writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine($"{DateTime.Now} : ");
                using (var xmlWriter = XmlWriter.Create(writer))
                {
                    message.WriteMessage(xmlWriter);
                }
                writer.WriteLine();
            }

        }

    }


}