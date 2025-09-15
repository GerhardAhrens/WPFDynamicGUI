//-----------------------------------------------------------------------
// <copyright file="ISubscription.cs" company="PTA">
//     Class: ISubscription
//     Copyright © PTA GmbH 2016
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@contractors.roche.com</email>
// <date>1.1.2016</date>
//
// <summary>Class with ISubscription Interface Definition</summary>
//-----------------------------------------------------------------------

namespace WPFDynamicGUI.Core
{
    using System;

    public interface ISubscription<TPayload> : IDisposable where TPayload : IPayload
    {
        Action<TPayload> Action { get; }

        IEventAggregator EventAggregator { get; }
    }
}