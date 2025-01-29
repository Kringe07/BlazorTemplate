using ProjectName.DataAccess.Enums;

namespace ProjectName.DataAccess.Entities;

public class Admin : User
{
    public override RoleType RoleType => RoleType.Admin;
}