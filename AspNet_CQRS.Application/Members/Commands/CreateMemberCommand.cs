using AspNet_CQRS.Application.Members.Notifications;
using AspNet_CQRS.Domain.Astractions;
using AspNet_CQRS.Domain.Entities;
using AspNet_CQRS.Infrastructure.Messages;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AspNet_CQRS.Application.Members.Commands
{
    public class CreateMemberCommand : MemberCommandBase
    {
        public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Member>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMediator _mediator;
            private readonly IValidator<CreateMemberCommand> _validator;
            private readonly IRabbitMQProducer _rabitMQProducer;
            private readonly IDistributedCache _redisCache;
            public CreateMemberCommandHandler(IUnitOfWork unitOfWork,
                                              IMediator mediator, 
                                              IValidator<CreateMemberCommand> validator,
                                              IRabbitMQProducer rabitMQProducer,
                                              IDistributedCache redisCache)
            {
                _unitOfWork = unitOfWork;
                _mediator = mediator;
                _validator = validator;
                _rabitMQProducer = rabitMQProducer;
                _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));

            }
            public async Task<Member> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
            {

                _validator.ValidateAndThrow(request);

                var newMember = new Member(request.FirstName, request.LastName, request.Gender, request.Email, request.IsActive);

                await _unitOfWork.MemberRepository.AddMember(newMember);
                await _unitOfWork.CommitAsync();

                await _mediator.Publish(new MemberCreatedNotification(newMember), cancellationToken);
              
                _rabitMQProducer.SendMemberMessage(newMember);

                await _redisCache.SetStringAsync($"{request.Id}",JsonSerializer.Serialize(newMember));

                return newMember;
            }
        }

    }
}
