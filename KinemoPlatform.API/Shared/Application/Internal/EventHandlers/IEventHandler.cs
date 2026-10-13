using KinemoPlatform.API.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace KinemoPlatform.API.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent>: INotificationHandler<TEvent> where TEvent: IEvent
{
    
}