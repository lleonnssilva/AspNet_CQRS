using MediatR;
using Microsoft.Extensions.Logging;

namespace AspNet_CQRS.Application.Members.Notifications
{
    public class MemberCreatedSMSHandler : INotificationHandler<MemberCreatedNotification>
    {
        private readonly ILogger<MemberCreatedEmailHandler>? _logger;
        public MemberCreatedSMSHandler(ILogger<MemberCreatedEmailHandler>? logger)
        {
            _logger = logger;
        }

        public Task Handle(MemberCreatedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Confirmação Sms sent for {notification.Member.FirstName}");

            return Task.CompletedTask;
        }
    }
}
