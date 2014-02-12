public class ObjectBase
{
    public bool RequiresDeletion { get; set; }
    public int Id { get; set; }
    public bool IsNew { get; private set; }
}