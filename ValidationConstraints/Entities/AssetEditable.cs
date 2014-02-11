using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationConstraintsObjectModel.Contracts;

namespace ValidationConstraintsObjectModel.Entities
{
    public class AssetEditable  : IRepositoryWorkflowEntity
    {
        public int Id { get; set; }
        public bool HandlesQuotes { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public AssetType AssetType { get; set; }
        public AssetStatus AssetStatus { get; set; }
    }
}
