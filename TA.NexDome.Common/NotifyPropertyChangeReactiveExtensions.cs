// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;

namespace TA.NexDome.Common
    {
    public static class NotifyPropertyChangeReactiveExtensions
        {
        // Returns the values of property (an Expression) as they
        // change, starting with the current value
        /// <summary>
        ///     Gets an observable sequence that produces a new value whenever the value of a property
        ///     (member expression) changes, starting with the current value.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="property">The property.</param>
        /// <returns>IObservable&lt;TValue&gt;.</returns>
        /// <exception cref="System.ArgumentException">
        ///     property must directly access " + "a property of the source
        /// </exception>
        public static IObservable<TValue> GetObservableValueFor<TSource, TValue>(
            this TSource source,
            Expression<Func<TSource, TValue>> property)
            where TSource : INotifyPropertyChanged
            {
            Contract.Requires(property != null);
            var memberExpression = property.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException("property must directly access " + "a property of the source");

            string propertyName = memberExpression.Member.Name;

            var accessor = property.Compile();

            return GetPropertyChangedEvents(source).Where(x => x.EventArgs.PropertyName == propertyName)
                .Select(x => accessor(source)).StartWith(accessor(source));
            }

        // This is a wrapper around FromEvent(PropertyChanged)
        public static IObservable<EventPattern<PropertyChangedEventArgs>> GetPropertyChangedEvents(
            this INotifyPropertyChanged source)
            {
            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => handler.Invoke,
                handler => source.PropertyChanged += handler,
                handler => source.PropertyChanged -= handler);
            }

        public static IDisposable Subscribe<TSource, TValue>(
            this TSource source,
            Expression<Func<TSource, TValue>> property,
            Action<TValue> observer)
            where TSource : INotifyPropertyChanged
            {
            Contract.Requires(property != null);
            return GetObservableValueFor<TSource, TValue>(source, property).Subscribe(observer);
            }

        public static IDisposable Subscribe<TSource, TValue>(
            this TSource source,
            Expression<Func<TSource, TValue>> property,
            Action<TValue> observer,
            Action onCompleted)
            where TSource : INotifyPropertyChanged
            {
            return GetObservableValueFor<TSource, TValue>(source, property).Subscribe(observer, onCompleted);
            }

        public static IDisposable Subscribe<TSource, TValue>(
            this TSource source,
            Expression<Func<TSource, TValue>> property,
            Action<TValue> observer,
            Action<Exception> onException)
            where TSource : INotifyPropertyChanged
            {
            return GetObservableValueFor<TSource, TValue>(source, property).Subscribe(observer, onException);
            }

        public static IDisposable Subscribe<TSource, TValue>(
            this TSource source,
            Expression<Func<TSource, TValue>> property,
            Action<TValue> observer,
            Action<Exception> onException,
            Action onCompleted)
            where TSource : INotifyPropertyChanged
            {
            return GetObservableValueFor<TSource, TValue>(source, property).Subscribe(observer, onException, onCompleted);
            }
        }
    }