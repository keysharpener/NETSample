using ValidationConstraintsObjectModel.Contracts;

namespace ValidationConstraintsObjectModel.Entities
{
    public class AssetEditable  : IRepositoryWorkflowEntity
    {
        public int Id { get; set; }
        public bool HandlingQuotes { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public AssetType AssetType { get; set; }
        public AssetStatus Status { get; set; }
    }
}
