// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: CommunicationsStackBuilder.cs  Last modified: 2018-03-02@00:33 by Tim Long

using System;
using TA.Ascom.ReactiveCommunications;

namespace TA.NexDome.DeviceInterface
    {
    /// <summary>
    ///     Factory methods for building the Reactive communicatiosn stack
    /// </summary>
    internal static class CommunicationsStackBuilder
        {
        public static ICommunicationChannel BuildChannel(DeviceEndpoint endpoint)
            {
            if (endpoint is SerialDeviceEndpoint)
                return new SerialCommunicationChannel(endpoint);
            throw new NotSupportedException($"There is no supported channel type for the endpoint: {endpoint}")
                {
                Data = {["endpoint"] = endpoint}
                };
            }

        public static TransactionObserver BuildTransactionObserver(ICommunicationChannel channel)
            {
            return new TransactionObserver(channel);
            }

        public static ITransactionProcessor BuildTransactionProcessor(TransactionObserver observer)
            {
            var processor = new ReactiveTransactionProcessor();
            processor.SubscribeTransactionObserver(observer);
            return processor;
            }
        }
    }