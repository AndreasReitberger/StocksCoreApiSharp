﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AndreasReitberger.Stocks.Utilities
{
    // https://learn.microsoft.com/de-de/xamarin/xamarin-forms/data-cloud/data/databases
    public class AsyncLazy<T>
    {
        readonly Lazy<Task<T>> instance;

        public AsyncLazy(Func<T> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public AsyncLazy(Func<Task<T>> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return instance.Value.GetAwaiter();
        }
    }
    /*
    public class AsyncLazy<S ,T>
    {
        readonly Lazy<Task<T>> instance;

        public AsyncLazy(Func<object, T> factory, object parameter)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(() => factory(parameter)));
        }

        public AsyncLazy(Func<object, Task<T>> factory, object parameter)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(() => factory(parameter)));
        }
        
        public TaskAwaiter<T> GetAwaiter()
        {
            return instance.Value.GetAwaiter();
        }
    }
    */
}
