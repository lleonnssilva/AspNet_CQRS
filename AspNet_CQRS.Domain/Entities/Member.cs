using AspNet_CQRS.Domain.Validation;
using System.Text.Json.Serialization;

namespace AspNet_CQRS.Domain.Entities
{
    public sealed class Member : Entity
    {
        public string? FirstName { get;  set; }
        public string? LastName { get;  set; }
        public string? Gender { get;  set; }
        public string? Email { get;  set; }
        public bool? IsActive { get;  set; }

        public Member(string firstname, string lastname, string gender, string email, bool? active)
        {
            ValidateDomain(firstname, lastname, gender, email, active);
        }

        [JsonConstructor]
        public Member() { }

       
        public Member(int id, string firstname, string lastname, string gender, string email, bool? active)
        {
            DomainValidation.When(id < 0, "Invalid Id value.");
            Id = id;
            ValidateDomain(firstname, lastname, gender, email, active);
        }

        public void Update(string firstname, string lastname, string gender, string email, bool? active)
        {
            ValidateDomain(firstname, lastname, gender, email, active);
        }

        private void ValidateDomain(string firstname, string lastname, string gender,
            string email, bool? active)
        {
            DomainValidation.When(string.IsNullOrEmpty(firstname),
                "Nome inválido. Nome é obrigatório.");

            DomainValidation.When(firstname.Length < 3,
                "Nome inválido, muito curto, mínimo de 3 caracteres");

            DomainValidation.When(string.IsNullOrEmpty(lastname),
                "Sobrenome inválido. Sobrenome é obrigatório.");

            DomainValidation.When(lastname.Length < 3,
                "Sobrenome inválido, muito curto, mínimo de 3 caracteres");

            DomainValidation.When(email?.Length > 250,
                "E-mail inválido, muito longo, máximo 250 caracteres");

            DomainValidation.When(email?.Length < 6,
                "E-mail inválido, muito curto, mínimo de 6 caracteres");

            DomainValidation.When(string.IsNullOrEmpty(gender),
               "Gênero inválido, gênero é obrigatório");

            DomainValidation.When(!active.HasValue,
                "Deve definir a atividade");

            FirstName = firstname;
            LastName = lastname;
            Gender = gender;
            Email = email;
            IsActive = active;
        }
    }

}
