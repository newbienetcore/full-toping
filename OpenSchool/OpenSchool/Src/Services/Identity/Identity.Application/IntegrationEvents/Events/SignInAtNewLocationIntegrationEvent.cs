using MessageBroker.Abstractions.Events;
using SharedKernel.Contracts;

namespace Identity.Application.IntegrationEvents.Events;

public record SignInAtNewLocationIntegrationEvent(TokenUser TokenUser) : IntegrationEvent;

