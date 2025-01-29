using ProjectName.DataAccess.Enums;

namespace ProjectName.DataAccess.Entities;

public class Customer : User
{
    public override RoleType RoleType => RoleType.Customer;
}
