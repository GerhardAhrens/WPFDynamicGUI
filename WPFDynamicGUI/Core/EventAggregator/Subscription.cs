//-----------------------------------------------------------------------
// <copyright file="Subscription.cs" company="PTA">
//     Class: Subscription
//     Copyright © PTA GmbH 2016
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@contractors.roche.com</email>
// <date>1.1.2016</date>
//
// <summary>Class with Subscription Definition</summary>
//-----------------------------------------------------------------------

namespace WPFDynamicGUI.Core
{
    using System;

    public class Subscription<TMessage> : ISubscription<TMessage> where TMessage : IPayload
    {
        public Subscription(IEventAggregator eventAggregator, Action<TMessage> action)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.EventAggregator = eventAggregator;
            this.Action = action;
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>The Action.</value>
        public Action<TMessage> Action { get; private set; }

        /// <summary>
        /// Gets the event aggregator.
        /// </summary>
        /// <value>The event aggregator.</value>
        public IEventAggregator EventAggregator { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.EventAggregator.UnSubscribe(this);
            }
        }
    }
}