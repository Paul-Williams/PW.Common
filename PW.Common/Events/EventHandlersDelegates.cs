using System;

namespace PW.Events;

/// <summary>
/// Delegate used to handle events with a strongly-typed payload only. No sender, just data of type <typeparamref name="TPayload"/>.
/// </summary>
/// <typeparam name="TPayload">The type event's payload.</typeparam>
public delegate void PayloadEventHandler<TPayload>(TPayload payload);


// Source: https://stackoverflow.com/questions/1437699/in-a-c-sharp-event-handler-why-must-the-sender-parameter-be-an-object
/// <summary>
/// Delegate used to handle events with a strongly-typed sender.
/// </summary>
/// <typeparam name="TSender">The type of the sender.</typeparam>
/// <typeparam name="TArgs">The type of the event arguments.</typeparam>
/// <param name="sender">The control where the event originated.</param>
/// <param name="e">Any event arguments.</param>
public delegate void TypedSenderEventHandler<TSender, TArgs>(TSender sender, TArgs e) where TArgs : EventArgs;

