﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SESDAD.CommonTypes;

namespace SESDAD.MessageBroker {
    public class BrokerRemoteService : MarshalByRefObject, IBrokerRemoteService {
        MessageBroker broker;

        public BrokerRemoteService(MessageBroker broker) : base() {
            this.broker = broker;
        }

        public void Publish(String processName, String processURL, Entry entry) {
            broker.ForwardEntry(processName, processURL, entry);
        }

        public void Subscribe(String processName, String processURL, String topicName) {
            broker.registerSubscription(processName, processURL, topicName);
        }

        public void Unsubscribe(String processName, String processURL, String topicName) {
            broker.removeSubscription(processName, processURL, topicName);
        }

        public void RegisterBroker(String procesName, String processURL) {
            //broker.AddChild(String processName, String processURL);
        }
    }
}