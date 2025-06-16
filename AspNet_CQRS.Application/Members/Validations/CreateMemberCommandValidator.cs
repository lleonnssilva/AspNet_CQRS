using AspNet_CQRS.Application.Members.Commands;
using FluentValidation;

namespace AspNet_CQRS.Application.Members.Validations
{
    public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
    {
        public CreateMemberCommandValidator()
        {
            RuleFor(c => c.FirstName)
              .NotEmpty()
              .WithMessage("Certifique-se de ter inserido o Nome")
              .Length(4, 100)
              .WithMessage("O Primeiro Nome deve ter entre 4 e 150 caracteres");

            RuleFor(c => c.LastName)
               .NotEmpty()
               .WithMessage("Por favor, certifique-se de ter inserido o Sobrenome")
               .Length(4, 100)
               .WithMessage("O Sobrenome deve ter entre 4 e 150 caracteres");


            RuleFor(c => c.Gender)
                .NotEmpty()
                .WithMessage("O gênero deve ser informado")
                .MinimumLength(4)
                .WithMessage("O gênero deve ter uma informação válida");

            RuleFor(c => c.Email)
               .NotEmpty()
               .EmailAddress();

            RuleFor(x => x.IsActive)
                .NotNull()
                .NotEmpty()
                .WithMessage("Certifique-se de ter inserido o status");
        }
    }

}
