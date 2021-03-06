﻿using Autofac.Extras.IocManager;

using Stove.Events.Bus;
using Stove.Events.Bus.Handlers;

using Xunit;

namespace Stove.Tests.SampleApplication
{
    public class EventBus_Tests : SampleApplicationTestBase
    {
        public EventBus_Tests()
        {
            Building(builder => { }).Ok();
        }

        [Fact]
        public void one_eventdata_with_multiple_handler()
        {
            The<IEventBus>().Publish(new SomeEvent
            {
                ExecutionCount = 0
            }, new Headers());

            The<IEventBus>().Publish(new SomeEvent2
            {
                ExecutionCount = 0
            }, new Headers());
        }

        [Fact]
        public void multiple_same_event_multiple_event_handler()
        {
            The<IEventBus>().Publish(new ProductCreatedEvent(12), new Headers());
        }

        [Fact]
        public void inherited_event_should_work()
        {
            The<IEventBus>().Publish(new InheritedEvent(16), new Headers());
        }

        public class ProductCreatedEvent : Event
        {
            public int ProductId;

            public ProductCreatedEvent(int productId)
            {
                ProductId = productId;
            }
        }

        public class InheritedEvent : ProductCreatedEvent
        {
            public InheritedEvent(int productId) : base(productId)
            {
            }
        }

        public class FirstEventHandler : IEventHandler<ProductCreatedEvent>, ITransientDependency
        {
            public void Handle(ProductCreatedEvent @event, Headers headers)
            {
            }
        }

        public class SecondEventHandler : IEventHandler<ProductCreatedEvent>, ITransientDependency
        {
            public void Handle(ProductCreatedEvent @event, Headers headers)
            {
            }
        }

        public class InheritedEventHandler : IEventHandler<InheritedEvent>, ITransientDependency
        {
            public void Handle(InheritedEvent @event, Headers headers)
            {
            }
        }

        public class SomeEvent : Event
        {
            public int ExecutionCount { get; set; }
        }

        public class SomeEvent2 : Event
        {
            public int ExecutionCount { get; set; }
        }

        public class SomeEventHandler : IEventHandler<SomeEvent>, IEventHandler<SomeEvent2>, ITransientDependency
        {
            public void Handle(SomeEvent @event, Headers headers)
            {
                @event.ExecutionCount++;
            }

            public void Handle(SomeEvent2 @event, Headers headers)
            {
                @event.ExecutionCount++;
            }
        }
    }
}
