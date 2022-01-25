using System;

namespace FarmFresh.Backend.Shared
{
    public class ProductListInput:BaseListInput
    {
        public string Category { get; set; } = GlobalConstants.CategoryNewKeyword;
    }
}
