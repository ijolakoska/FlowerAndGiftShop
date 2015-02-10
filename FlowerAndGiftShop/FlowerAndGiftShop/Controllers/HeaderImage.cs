using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowerAndGiftShop.Controllers
{
    class HeaderImage
    {
        public byte [] ImageData;
        public string ImageName;
        public bool IsActive;

        public LinkedList<HeaderImage> imageRepository = new LinkedList<HeaderImage>();

        internal void AddHeaderImage(HeaderImage headerImage)
        {
            this.imageRepository.AddLast(headerImage);
        }
    }
}
