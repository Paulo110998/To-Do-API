using Microsoft.AspNetCore.Authorization;

namespace TO_DO___API.Authorization;

public class AuthenticationUser : IAuthorizationRequirement
{
    public AuthenticationUser()
    {

    }
}