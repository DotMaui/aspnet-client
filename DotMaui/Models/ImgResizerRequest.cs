/// <summary>
/// 
/// </summary>
namespace DotMaui.Models
{
    public class ImgResizerRequest
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public ImgResizerRequest(string url, int width = 0, int height = 0)
        {
            this.Url = url;
            this.Width = width;
            this.Height = height;
        }

    }
}
