namespace ProjectName.DataAccess.Entities;

public interface IArchivableEntity
{
    bool IsArchived { get; set; }
}