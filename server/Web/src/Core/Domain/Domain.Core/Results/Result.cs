﻿using Domain.Core.Primitives;

namespace Domain.Core.Results;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException();


        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException();


        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Create<TValue>(TValue? value, Error error)
        where TValue : class =>
        value ?? Failure<TValue>(error);
    public static Result<TValue> Update<TValue>(TValue? value, Error error)
        where TValue : class =>
        value ?? Failure<TValue>(error);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) => new(default!, false, error);

    public static Result FirstFailureOrSuccess(params Result[] results)
    {
        foreach (var result in results)
        {
            if (result.IsFailure)
            {
                return result;
            }
        }

        return Success();
    }
}

public class Result<TValue> : Result
{
    private readonly TValue _value;

    protected internal Result(TValue value, bool isSuccess, Error error)
        : base(isSuccess, error) =>
        _value = value;

    public TValue Value()
    {
        if (IsFailure)
            throw new InvalidOperationException();

        return _value;
    }

    public static implicit operator Result<TValue>(TValue value) => Success(value);
}