using Microsoft.AspNetCore.Mvc;
using server.Helpers;

public class Authorization : ControllerBase
{
    public int ValidateUser(string jwt)
    {
        try
        {
            var token = JWTService.Verify(jwt);
            int userId = int.Parse(token.Issuer);
            return userId;
        }
        catch
        {
            return -1;
        }
    }
}