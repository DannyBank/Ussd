using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.Data.Extensions
{
    public static class CollectionExtensions
    {
        public static  bool IsEnumerableType(this Type type)
        {
            return (type.GetInterface(nameof(IEnumerable)) != null);
        }
        public static  bool IsCollectionType(this Type type)
        {
            return (type.GetInterface(nameof(ICollection)) != null);
        }
        public static Type GetEnumeratedType<T>(this IEnumerable<T> _)
        {
            return typeof(T);
        }
        public static bool IsSingle<T>(this IEnumerable<T> list)
        {
            try
            {
                if(list is null)
                    throw new ArgumentNullException(nameof(list));
                return list.Single() != null;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            try
            {
                return list is null || !list.Any();
            }
            catch
            {
                return false;
            }
        }
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            int i = 0;
            var splits = from item in list
                         group item by i++ % parts into part
                         select part.AsEnumerable();
            return splits;
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action, CancellationToken cancellationToken) 
        {
            var asyncEnumerable = (IAsyncEnumerable<T>)enumerable;
            await using var enumerator = asyncEnumerable.GetAsyncEnumerator();
            if (await enumerator.MoveNextAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
            {
                ValueTask<bool> moveNextTask;
                do
                {
                    var current = enumerator.Current;
                    moveNextTask = enumerator.MoveNextAsync(cancellationToken);
                    await action(current); //now with await
                }
                while (await moveNextTask.ConfigureAwait(continueOnCapturedContext: false));
            }
        }
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action) //Now with Func returning Task
        {
            var asyncEnumerable = (IAsyncEnumerable<T>)enumerable;
            await using var enumerator = asyncEnumerable.GetAsyncEnumerator();
            if (await enumerator.MoveNextAsync().ConfigureAwait(continueOnCapturedContext: false))
            {
                ValueTask<bool> moveNextTask;
                do
                {
                    var current = enumerator.Current;
                    moveNextTask =  enumerator.MoveNextAsync(CancellationToken.None);
                    await action(current); //now with await
                }
                while (await moveNextTask.ConfigureAwait(continueOnCapturedContext: false));
            }
        }
    }
}