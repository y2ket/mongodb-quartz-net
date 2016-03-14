﻿using System;
using MongoDB.Driver;

namespace Quartz.Spi.MongoDbJobStore.Repositories
{
    internal abstract class BaseRepository<TDocument>
    {
        protected BaseRepository(IMongoDatabase database, string collectionPrefix = null)
        {
            var collectionName = GetCollectionName();
            if (!string.IsNullOrEmpty(collectionPrefix))
            {
                collectionName = $"{collectionPrefix}.{collectionName}";
            }

            Collection = database.GetCollection<TDocument>(collectionName);
        }

        protected IMongoCollection<TDocument> Collection { get; }

        protected FilterDefinitionBuilder<TDocument> FilterBuilder => Builders<TDocument>.Filter;

        protected UpdateDefinitionBuilder<TDocument> UpdateBuilder => Builders<TDocument>.Update;

        protected SortDefinitionBuilder<TDocument> SortBuilder => Builders<TDocument>.Sort;

        protected ProjectionDefinitionBuilder<TDocument> ProjectionBuilder => Builders<TDocument>.Projection;

        protected IndexKeysDefinitionBuilder<TDocument> IndexBuilder => Builders<TDocument>.IndexKeys;

        /// <summary>
        ///     Determines the collectionname
        /// </summary>
        /// <returns>Returns the collectionname.</returns>
        private string GetCollectionName()
        {
            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = Attribute.GetCustomAttribute(GetType(), typeof (CollectionName));
            var collectionname = att != null ? ((CollectionName) att).Name : typeof (TDocument).Name;

            return collectionname;
        }
    }
}