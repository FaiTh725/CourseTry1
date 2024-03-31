namespace CourseTry1.Domain.Enum
{
    public enum StatusCode
    {
        RegisteredUser = 1,
        UnRegistered = 2,
        IncorrectPassword = 3,
        BadFile = 4,
        FileExcist = 5,
        DontFindGroup,
        DontFindShedule,

        Ok = 200,
        BadRequest = 400,
    }
}
