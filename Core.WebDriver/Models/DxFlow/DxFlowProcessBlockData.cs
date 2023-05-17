using System.Collections.Generic;

namespace Taf.UI.Core.Models
{
    public class DxFlowProcessBlockData
    {
        public List<object> BlockData { get; set; } = new List<object>();

        public DxFlowProcessBlockData AddBlockData(object data)
        {
            BlockData.Add(data);

            return this;
        }

        public DxFlowProcessBlockData AddImageData(string filePath="", string originalFileName="")
        {
            TafEmImageData imageData = new TafEmImageData()
            {
                FilePath = filePath,
                OriginalFileName = originalFileName
            };

            return AddBlockData(imageData);
        }

        public void AddVideoData(string uri)
        {
            TafEmVideoBlockData videoData = new TafEmVideoBlockData()
            {
                VideoUrl = uri
            };

            AddBlockData(videoData);
        }
    }
}
