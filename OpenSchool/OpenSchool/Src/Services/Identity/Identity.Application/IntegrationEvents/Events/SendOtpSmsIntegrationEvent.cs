using MessageBroker.Abstractions.Events;

namespace Identity.Application.IntegrationEvents.Events;

public record SendOtpSmsIntegrationEvent(string Phone, string Otp) : IntegrationEvent;