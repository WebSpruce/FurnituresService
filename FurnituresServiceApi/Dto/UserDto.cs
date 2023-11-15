using Microsoft.AspNetCore.Identity;

namespace FurnituresServiceApi.Dto
{
    public class UserDto
    {
        [ProtectedPersonalData]
        public virtual string? Email { get; set; }
        [PersonalData]
        public virtual bool EmailConfirmed { get; set; }
        [ProtectedPersonalData]
        public virtual string? PhoneNumber { get; set; }
    }
}
