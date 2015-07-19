﻿using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eddi.Data.Core;

namespace Eddi.Data.MongoDb
{
    public class MongoDbCollectionRepository : ICollectionRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;

        public MongoDbCollectionRepository(string connectionString, string database, string collection)
        {
            _database = new MongoClient(connectionString)
                .GetDatabase(database);
            _collection = _database.GetCollection<BsonDocument>(collection);
            _collectionName = collection;
        }

        public async Task BatchSaveAsync(IEnumerable<string> jsonCollection)
        {
            foreach (var doc in jsonCollection
                .Where(json => !string.IsNullOrWhiteSpace(json))
                .Select(BsonDocument.Parse))
            {
                await _collection.InsertOneAsync(doc);
            }
        }

        public async Task DropCollectionAsync()
        {
            await _database.DropCollectionAsync(_collectionName);
        }

        public async Task SaveAsync(string jsonString)
        {
            var doc = BsonDocument.Parse(jsonString);
            await _collection.InsertOneAsync(doc);
        }
    }

    public class MongoDbCollectionRepository<T> : ICollectionRepository<T>
    {
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;

        public MongoDbCollectionRepository(string connectionString, string database, string collection)
        {
            _database = new MongoClient(connectionString)
                .GetDatabase(database);
            _collection = _database.GetCollection<T>(collection);
            _collectionName = collection;
        }

        public async Task BatchSaveAsync(IEnumerable<T> items)
        {
            foreach (var item in items
                .Where(item => item != null))
            {
                await _collection.InsertOneAsync(item);
            }
        }

        public async Task DropCollectionAsync()
        {
            await _database.DropCollectionAsync(_collectionName);
        }

        public async Task SaveAsync(T item)
        {
            await _collection.InsertOneAsync(item);
        }

        public async Task<IList<T>> FindAsync(Expression<Func<T, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).ToListAsync();
        }

        public async Task<IList<T>> FindAsync(Expression<Func<T, bool>> filterExpression, Expression<Func<T, object>> sortExpression, int sortDirection = 1)
        {
            if (sortDirection > 0)
                return await _collection.Find(filterExpression).SortBy(sortExpression).ToListAsync();
            else
                return await _collection.Find(filterExpression).SortByDescending(sortExpression).ToListAsync();
        }
    }
}