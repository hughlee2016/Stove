using System;

using FluentNHibernate.Mapping;

using Stove.Domain.Entities;

namespace Stove.NHibernate.EntityMappings
{
    /// <summary>
    ///     This class is base class to map entities to database tables.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Type of primary key of the entity</typeparam>
    public abstract class EntityMap<TEntity, TPrimaryKey> : ClassMap<TEntity> where TEntity : IEntity<TPrimaryKey>
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="tableName">Table name</param>
        protected EntityMap(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName)) //TODO: Use code contracts?
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            Table(tableName);
            Id(x => x.Id);

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                Where("IsDeleted = 0"); //TODO: Test with other DBMS then SQL Server
            }
        }
    }
}
