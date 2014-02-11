public class ObjectBase
{
    public bool RequiresUpdate { get; set; }
    public bool RequiresDeletion { get; set; }
    public int Id { get; private set; }
    public bool IsNew { get; private set; }
}