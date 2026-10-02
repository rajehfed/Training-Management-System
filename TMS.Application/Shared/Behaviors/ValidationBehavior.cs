using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Shared.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if(failures.Count != 0)
        {
            var errorMessage = string.Join(" | ", failures.Select(f => f.ErrorMessage));

            var validationError = new ErrorBase("ValidationError", errorMessage, ErrorType.ValidationError);

            return (TResponse)ResultBase.Failure(validationError);
        }

        return await next(cancellationToken);
    }
}
