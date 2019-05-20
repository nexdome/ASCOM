// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: ITransactionProcessorFactory.cs  Last modified: 2018-03-02@00:31 by Tim Long

using TA.Ascom.ReactiveCommunications;

namespace TA.NexDome.DeviceInterface
    {
    /// <summary>
    ///     An object that is capable of creating and returning an <see cref="ITransactionProcessor" />.
    /// </summary>
    public interface ITransactionProcessorFactory
        {
        /// <summary>
        ///     The communications channel used by a created <see cref="ITransactionProcessor" />.
        /// </summary>
        ICommunicationChannel Channel { get; }

        /// <summary>
        ///     The device endpoint used by a created <see cref="ITransactionProcessor" />.
        /// </summary>
        DeviceEndpoint Endpoint { get; }

        /// <summary>
        ///     Creates the transaction processor ready for use. Also creates and initialises the
        ///     device endpoint and the communications channel and opens the channel.
        /// </summary>
        /// <returns>ITransactionProcessor.</returns>
        ITransactionProcessor CreateTransactionProcessor();

        /// <summary>
        ///     Destroys the transaction processor and its dependencies. Ensures that the
        ///     <see cref="ReactiveTransactionProcessorFactory.Channel" /> is closed. Once this method has been called, the
        ///     <see cref="ReactiveTransactionProcessorFactory.Channel" /> and
        ///     <see cref="ReactiveTransactionProcessorFactory.Endpoint" /> properties will be null.
        /// </summary>
        void DestroyTransactionProcessor();
        }
    }