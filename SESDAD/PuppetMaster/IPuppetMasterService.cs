﻿using System;

namespace SESDAD.PuppetMaster {

    //<summary>
    // Type of Logging Level
    //</summary>
    public interface IPuppetMasterService {
        void SetPolicies(RoutingPolicyType newRoutingPolicy, OrderingType newOrdering, LoggingLevelType newLoggingLevel);
        void ExecuteSiteCommand(String siteName, String parentName);
        void ExecuteBrokerCommand(String brokerName, String SiteName, String urlName);
        void ExecutePublisherCommand(String publisherName, String SiteName, String urlName);
        void ExecuteSubscriberCommand(String subscriberName, String SiteName, String urlName);
        void ExecuteFloodingRoutingPolicyCommand();
        void ExecuteFilterRoutingPolicyCommand();
        void ExecuteNoOrderingCommand();
        void ExecuteFIFOOrderingCommand();
        void ExecuteTotalOrderingCommand();
        void ExecuteSubscribeCommand(String subscriberName, String topicName);
        void ExecuteUnsubscribeCommand(String subscriberName, String topicName);
        void ExecutePublishCommand(String publisherName, int publishTimes, String topicName, int intervalTime);
        String ExecuteStatusCommand();
        void ExecuteCrashCommand(String processName);
        void ExecuteFreezeCommand(String processName);
        void ExecuteUnfreezeCommand(String processName);
        void ExecuteWaitCommand(int waitingTime);
        void ExecuteFullLoggingLevelCommand();
        void ExecuteLightLoggingLevelCommand();
    }
}