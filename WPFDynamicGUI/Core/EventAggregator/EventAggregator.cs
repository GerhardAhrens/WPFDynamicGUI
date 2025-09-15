//-----------------------------------------------------------------------
// <copyright file="EventAggregator.cs" company="PTA">
//     Class: EventAggregator
//     Copyright © PTA GmbH 2016
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@contractors.roche.com</email>
// <date>1.1.2016</date>
//
// <summary>Class with EventAggregator Definition</summary>
//-----------------------------------------------------------------------

namespace WPFDynamicGUI.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class EventAggregator : IEventAggregator
    {
        /// <summary>
        /// The subscriptions.
        /// </summary>
        private static readonly IDictionary<Type, IList> Subscriptions = new Dictionary<Type, IList>();

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="TPayload">The type of the message.</typeparam>
        /// <param name="payload">The payload.</param>
        public void Publish<TPayload>(TPayload payload) where TPayload : IPayload
        {
            if (payload == null)
            {
                throw new ArgumentNullException("payload");
            }

            Type messageType = typeof(TPayload);

            if (Subscriptions.ContainsKey(messageType))
            {
                var subscriptionList
                    = new List<ISubscription<TPayload>>(Subscriptions[messageType].Cast<ISubscription<TPayload>>());

                foreach (var subscription in subscriptionList)
                {
                    subscription.Action(payload);
                }
            }
        }

        /// <summary>
        /// Subscribes the specified action.
        /// </summary>
        /// <typeparam name="TPayload">The type of the message.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>The subscription.</returns>
        public ISubscription<TPayload> Subscribe<TPayload>(Action<TPayload> action) where TPayload : IPayload
        {
            Type messageType = typeof(TPayload);

            var subscription = new Subscription<TPayload>(this, action);

            if (Subscriptions.ContainsKey(messageType))
            {
                Subscriptions[messageType].Add(subscription);
            }
            else
            {
                Subscriptions.Add(messageType, new List<ISubscription<TPayload>> { subscription });
            }

            return subscription;
        }

        /// <summary>
        /// Unsubscribe from event.
        /// </summary>
        /// <typeparam name="TPayload">The type of the message.</typeparam>
        /// <param name="subscription">The subscription.</param>
        public void UnSubscribe<TPayload>(ISubscription<TPayload> subscription) where TPayload : IPayload
        {
            Type messageType = typeof(TPayload);

            if (Subscriptions.ContainsKey(messageType))
            {
                Subscriptions[messageType].Remove(subscription);
            }
        }

        /// <summary>
        /// Unsubscribe from event.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <param name="action">The action.</param>
        public void UnSubscribe<TPayload>(Action<TPayload> action) where TPayload : IPayload
        {
            Type messageType = typeof(TPayload);

            if (Subscriptions.ContainsKey(messageType))
            {
                Subscriptions[messageType].Remove(Subscriptions[messageType]);
            }
        }

        /// <summary>
        /// Clears all subscriptions.
        /// </summary>
        public void ClearAllSubscriptions()
        {
            this.ClearAllSubscriptions(null);
        }

        /// <summary>
        /// Clears all subscriptions.
        /// </summary>
        /// <param name="excepTPayloads">The except messages.</param>
        public void ClearAllSubscriptions(Type[] excepTPayloads)
        {
            foreach (var messageSubscriptions in new Dictionary<Type, IList>(Subscriptions))
            {
                bool canDelete = true;
                if (excepTPayloads != null)
                {
                    canDelete = !excepTPayloads.Contains(messageSubscriptions.Key);
                }

                if (canDelete)
                {
                    Subscriptions.Remove(messageSubscriptions);
                }
            }
        }
    }
}