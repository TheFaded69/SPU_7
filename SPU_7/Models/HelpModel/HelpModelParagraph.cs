using System.Collections.ObjectModel;

namespace SPU_7.Models.HelpModel
{
    public class HelpModelParagraph
    {
        public ObservableCollection<HelpModelParagraph> InternalParagraph { get; }

        public string ParagraphName { get; set; }

        public HelpModelContent Content { get; set; }
    }
}
