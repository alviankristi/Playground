using PG.Model;

namespace PG.Api.DtoModels
{
    public class EditUserProfileDto : BaseDto<UserProfile>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UserProfileDto : EditUserProfileDto
    {
        public string Email { get; set; }

        public override void LoadFromEntity(UserProfile entity)
        {
            base.LoadFromEntity(entity);

            FirstName = entity.FirstName;
            LastName = entity.LastName;

            if (entity.AppUser != null)
            {
                Email = entity.AppUser.Email;
            }
        }
    }
}