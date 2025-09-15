//-----------------------------------------------------------------------
// <copyright file="IEventAggregator.cs" company="PTA">
//     Class: IEventAggregator
//     Copyright © PTA GmbH 2016
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@contractors.roche.com</email>
// <date>1.1.2016</date>
//
// <summary>Class with IEventAggregator Definition</summary>
//-----------------------------------------------------------------------

namespace WPFDynamicGUI.Core
{
    using System;

    public interface IEventAggregator
    {
        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="TPayload">The type of the message.</typeparam>
        /// <param name="payload">The payload.</param>
        void Publish<TPayload>(TPayload payload) where TPayload : IPayload;

        /// <summary>
        /// Subscribes the specified action.
        /// </summary>
        /// <typeparam name="TPayload">The type of the message.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>The subscription.</returns>
        ISubscription<TPayload> Subscribe<TPayload>(Action<TPayload> action) where TPayload : IPayload;

        /// <summary>
        /// Uns the subscribe.
        /// </summary>
        /// <typeparam name="TPayload">The type of the message.</typeparam>
        /// <param name="subscription">The subscription.</param>
        void UnSubscribe<TPayload>(ISubscription<TPayload> subscription) where TPayload : IPayload;

        /// <summary>
        /// Clears all subscriptions.
        /// </summary>
        void ClearAllSubscriptions();

        /// <summary>
        /// Clears all subscriptions.
        /// </summary>
        /// <param name="exceptMessages">The except messages.</param>
        void ClearAllSubscriptions(Type[] exceptMessages);
    }
}
