using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.CustomControls.Classes
{
    enum BoxSize
    {
        Small = 1,
        Medium = 2,
        Large = 3
    }

    internal static class Boxes
    {
        public static Dictionary<BoxSize, int> BoxSizeImage => new Dictionary<BoxSize, int>
        {
            { BoxSize.Small, 60 },
            { BoxSize.Medium, 75 },
            { BoxSize.Large, 90 }
        };

        public static Dictionary<BoxSize, string> BoxImages => new Dictionary<BoxSize, string>
        {
            { BoxSize.Small, @"PurchaseSizes\boxSmall.png" },
            { BoxSize.Medium, @"PurchaseSizes\boxMid.png" },
            { BoxSize.Large, @"PurchaseSizes\boxbig.png" }
        };

        public static Dictionary<BoxSize, float> BoxPrices => new Dictionary<BoxSize, float>
        {
            { BoxSize.Small, 15.99f },
            { BoxSize.Medium, 19.99f },
            { BoxSize.Large, 24.99f}
        };

        public static Dictionary<BoxSize, string> BoxSizes => new Dictionary<BoxSize, string>
        {
            { BoxSize.Small, "10x10x15" },
            { BoxSize.Medium, "15x15x30" },
            { BoxSize.Large, "30x30x50" }
        };

        public static Dictionary<BoxSize, string> BoxNames => new Dictionary<BoxSize, string>
        {
            { BoxSize.Small, "Mała" },
            { BoxSize.Medium, "Średnia" },
            { BoxSize.Large, "Duża" }
        };
    }
}
