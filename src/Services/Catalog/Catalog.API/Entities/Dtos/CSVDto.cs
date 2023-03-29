namespace Catalog.API.Entities.Dtos
{
    public class CSVDto
    {
        private List<Category> updateCategories;
        private List<Category> newCategories;
        private List<string> invalidEntries;
        private List<string> duplicatePartNumbers;

        public int UpdateCategoriesCount { get; private set; }
        public int NewCategoriesCount { get; private set; }
        public int InvalidEntriesCount { get; private set; }
        public int DuplicatePartNumbersCount { get; private set; }
        public List<Category> UpdateCategories
        {
            get => updateCategories; set
            {
                updateCategories = value;
                UpdateCategoriesCount = value.Count;
            }
        }
        public List<Category> NewCategories
        {
            get => newCategories; set
            {
                newCategories = value;
                NewCategoriesCount = value.Count;
            }
        }
        public List<string> InvalidEntries
        {
            get => invalidEntries; set
            {
                invalidEntries = value;
                InvalidEntriesCount = value.Count;
            }
        }
        public List<string> DuplicatePartNumbers
        {
            get => duplicatePartNumbers; set
            {
                duplicatePartNumbers = value;
                DuplicatePartNumbersCount = value.Count;
            }
        }
    }
}
