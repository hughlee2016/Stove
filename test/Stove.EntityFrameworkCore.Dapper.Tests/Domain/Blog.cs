﻿using System;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.EntityFrameworkCore.Dapper.Tests.Domain.Events;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Domain
{
    public class Blog : AggregateRoot, IHasCreationTime
    {
        public Blog()
        {
            Register<BlogUrlChangedEvent>(@event => { Url = @event.Url; });
        }

        public Blog(string name, string url)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            Name = name;
            Url = url;

            ApplyChange(
                new BlogCreatedEvent(name)
                );
        }

        public string Name { get; set; }

        public string Url { get; protected set; }

        public DateTime CreationTime { get; set; }

        public void ChangeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            ApplyChange(new BlogUrlChangedEvent(this, url));
        }
    }
}
