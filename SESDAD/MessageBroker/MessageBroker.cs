﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Net.Sockets;
using System.Threading;

using SESDAD.CommonTypes;


namespace SESDAD.MessageBroker {

    using SubscriberList = Dictionary<String,ISubscriberRemoteObject>; //typedef

    public class MessageBroker : Process {

        private String parentURL;
        EventRouter eventRouter;
        private IDictionary<String,SubscriberList> subscriptions;

        public MessageBroker(String processName, String siteName, String processURL, String parentURL)
                : base(processName, siteName, processURL) {
            eventRouter = new EventRouter();
            subscriptions = new Dictionary<String,SubscriberList>();
        }

        public override void Connect() {
            base.Connect();
             //connect to parent broker
           /* if (parentURL != null) {
                IBrokerRemoteService parentBroker = (IBrokerRemoteService)Activator.GetObject(
                    typeof(IBrokerRemoteService),
                    parentURL);
                parentBroker.RegisterBroker(processName, processURL);
            }*/           
            
            //make remote service available
            RemotingServices.Marshal(
                new BrokerRemoteService(this),
                serviceName,
                typeof(BrokerRemoteService));
        }


        public void registerSubscription(String processName, String processURL, String topicName) {
            SubscriberList subscriberList;

            //get subscriber remote object
            ISubscriberRemoteObject newSubscriber =
                (ISubscriberRemoteObject)Activator.GetObject(
                       typeof(ISubscriberRemoteObject),
                       processURL);
            //if there are no subs to that topic, create a new list of subscribers
            if (!subscriptions.TryGetValue(topicName, out subscriberList)) {
                subscriberList = new SubscriberList();
                subscriptions.Add(topicName, subscriberList);
            }
            //add the new subscriber
            subscriberList.Add(processName, newSubscriber);
            //notify parent to update
            Console.WriteLine("New subcriber: " + processName);
        }

        public void removeSubscription(String processName, String processURL, String topicName) {
            SubscriberList subscriberList;
            //get subscriber by name and remove it from the list
            if (subscriptions.TryGetValue(topicName, out subscriberList)) {
                subscriberList.Remove(processName);
            }
            //notify parent to update
            Console.WriteLine("Removed subcriber: " + processName);
        }

        public void ForwardEntry(String processName, String processURL, Entry entry) {
            SubscriberList subscriberList;
            String topicName = entry.getTopicName();

            if (subscriptions.TryGetValue(topicName, out subscriberList)) {
                foreach (KeyValuePair<String,ISubscriberRemoteObject> subscriber in subscriberList.ToList()) {
                    subscriber.Value.DeliverEntry(entry);
                }
            }
            //eventrouter.broadcast
            Console.WriteLine("Forwarding entry to all subscribers");
        }

        public void AddChild(String processName, String processURL) {
            
        }

        public void NotifyParent() { }

        public void Debug() {
            String debugMessage =
                "**********************************************" + "\n" +
                "* Hello, I'm a MessageBroker. Here's my info:" + "\n" +
                "* Process Name: " + processName + "\n" +
                "* Site Name: " + siteName + "\n" +
                "* Process URL: " + processURL + "\n" +
                "* Port Number: " + portNumber + "\n" +
                "* Service Name: " + serviceName + "\n" +
                "* Parent URL: " + parentURL + "\n" +
                "**********************************************" + "\n";
            Console.Write(debugMessage);
        }
    }

    class Program {
        static void Main(string[] args) {
            MessageBroker broker = new MessageBroker(args[0], args[1], args[2], args[3]);
            broker.Debug();
            broker.Connect();
            Console.ReadLine();
        }
    }
}