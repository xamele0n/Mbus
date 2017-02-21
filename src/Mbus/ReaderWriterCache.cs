using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus
{
    using System.Threading;

    /// <summary>
    /// The reader writer cache.
    /// </summary>
    /// <typeparam name="TKey">
    /// </typeparam>
    /// <typeparam name="TValue">
    /// </typeparam>
    public abstract class ReaderWriterCache<TKey, TValue>
    {
        /// <summary>
        /// The cache.
        /// </summary>
        private readonly Dictionary<TKey, TValue> cache;

        /// <summary>
        /// The reader writer lock.
        /// </summary>
        private readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderWriterCache{TKey,TValue}"/> class.
        /// </summary>
        protected ReaderWriterCache()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderWriterCache{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        protected ReaderWriterCache(IEqualityComparer<TKey> comparer)
        {
            this.cache = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        protected Dictionary<TKey, TValue> Cache
        {
            get
            {
                return this.cache;
            }
        }

        /// <summary>
        /// The fetch or create item.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="creator">
        /// The creator.
        /// </param>
        /// <returns>
        /// The <see cref="TValue"/>.
        /// </returns>
        protected TValue FetchOrCreateItem(TKey key, Func<TValue> creator)
        {
            // Passing the delegate as an argument allows the inline delegate to be static
            return FetchOrCreateItem(key, (innerCreator) => innerCreator(), creator);
        }

        /// <summary>
        /// The fetch or create item.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="creator">
        /// The creator.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <typeparam name="TArgument">
        /// </typeparam>
        /// <returns>
        /// The <see cref="TValue"/>.
        /// </returns>
        protected TValue FetchOrCreateItem<TArgument>(TKey key, Func<TArgument, TValue> creator, TArgument state)
        {
            // first, see if the item already exists in the cache
            this.readerWriterLock.EnterReadLock();
            try
            {
                TValue existingEntry;
                if (this.cache.TryGetValue(key, out existingEntry))
                {
                    return existingEntry;
                }
            }
            finally
            {
                this.readerWriterLock.ExitReadLock();
            }

            // insert the new item into the cache
            TValue newEntry = creator(state);
            this.readerWriterLock.EnterWriteLock();
            try
            {
                TValue existingEntry;
                if (this.cache.TryGetValue(key, out existingEntry))
                {
                    // another thread already inserted an item, so use that one
                    return existingEntry;
                }

                this.cache[key] = newEntry;
                return newEntry;
            }
            finally
            {
                this.readerWriterLock.ExitWriteLock();
            }
        }
    }

}
