﻿using Domain.Core.Primitives;

namespace Domain.Core.Results.Extensions;

public static class ResultExtensions
{
    public static Result<TValue> Ensure<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Error error)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value()) ? Result.Success(result.Value()) : Result.Failure<TValue>(error);
    }

    public static async Task<Result<TValue>> Ensure<TValue>(
        this Task<Result<TValue>> resultTask, Func<TValue, bool> predicate, Error error)
    {
        var result = await resultTask;

        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value()) ? Result.Success(result.Value()) : Result.Failure<TValue>(error);
    }

    public static async Task<Result<TValue>> Ensure<TValue>(
        this Result<TValue> result, Func<TValue, Task<bool>> predicate, Error error)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return await predicate(result.Value()) ? Result.Success(result.Value()) : Result.Failure<TValue>(error);
    }

    public static async Task<T> Match<T>(this Task<Result> resultTask, Func<T> onSuccess, Func<Result, T> onFailure)
    {
        var result = await resultTask;

        return result.Match(onSuccess, onFailure);
    }

    public static T Match<T>(this Result result, Func<T> onSuccess, Func<Result, T> onFailure)
        => result.IsSuccess ? onSuccess() : onFailure(result);

    public static async Task<T> Match<TValue, T>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, T> onSuccess,
        Func<Result, T> onFailure)
    {
        var result = await resultTask;

        return result.Match(onSuccess, onFailure);
    }

    public static T Match<TValue, T>(this Result<TValue> result, Func<TValue, T> onSuccess, Func<Result, T> onFailure)
        => result.IsSuccess ? onSuccess(result.Value()) : onFailure(result);

    public static Result<T> Map<T>(this Result result, Func<Result<T>> func)
        => result.IsSuccess ? func() : Result.Failure<T>(result.Error);

    public static Result<T> Map<TValue, T>(this Result<TValue> result, Func<TValue, T> func)
        => result.IsSuccess ? Result.Success(func(result.Value())) : Result.Failure<T>(result.Error);

    public static async Task<Result<T>> Map<TValue, T>(this Task<Result<TValue>> resultTask, Func<TValue, T> func)
    {
        var result = await resultTask;

        return result.IsSuccess ? Result.Success(func(result.Value())) : Result.Failure<T>(result.Error);
    }

    public static async Task<Result> Bind<TValue>(this Result<TValue> result, Func<TValue, Task<Result>> func)
        => result.IsSuccess ? await func(result.Value()) : Result.Failure(result.Error);

    public static async Task<Result<T>> Bind<TValue, T>(this Result<TValue> result, Func<TValue, Task<Result<T>>> func)
        => result.IsSuccess ? await func(result.Value()) : Result.Failure<T>(result.Error);

    public static async Task<Result<T>> Bind<TValue, T>(this Result<TValue> result, Func<TValue, Task<T>> func)
        where T : class
        => result.IsSuccess ? await func(result.Value()) : Result.Failure<T>(result.Error);

    public static async Task<Result<T>> Bind<TValue, T>(this Result<TValue> result, Func<TValue, Task<T?>> func,
        Error error)
        where T : class
    {
        if (result.IsFailure)
        {
            return Result.Failure<T>(result.Error);
        }

        var value = await func(result.Value());

        return value is null ? Result.Failure<T>(error) : Result.Success(value);
    }

    public static async Task<Result<T>> Bind<TValue, T>(this Task<Result<TValue>> resultTask,
        Func<TValue, Task<T>> func)
    {
        var result = await resultTask;

        return result.IsSuccess ? Result.Success(await func(result.Value())) : Result.Failure<T>(result.Error);
    }

    public static async Task<Result<T>> BindScalar<TValue, T>(this Result<TValue> result, Func<TValue, Task<T>> func)
        where T : struct
    {
        return result.IsSuccess ? await func(result.Value()) : Result.Failure<T>(result.Error);
    }

    public static async Task<Result> Tap<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> action)
    {
        var result = await resultTask;

        if (result.IsSuccess)
        {
            action(result.Value());
        }

        return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
    }

    public static async Task<Result> Tap<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> action)
    {
        var result = await resultTask;

        if (result.IsSuccess)
        {
            await action(result.Value());
        }

        return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
    }
}