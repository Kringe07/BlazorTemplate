using ProjectName.DataAccess.Enums;

namespace ProjectName.DataAccess.Entities;

public class Admin : User
{
    public override Role Role => Role.Admin;
}