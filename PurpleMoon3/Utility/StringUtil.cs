using System;
using System.Collections.Generic;
using System.Text;

namespace PurpleMoon3
{
    public static class StringUtil
    {
        private static char[] HexValues = new char[16] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static string HexToString(uint num)
        {
            int bytes = 1;
            if (num <= 255) { bytes = 1; }
            else if (num > 255 && num <= 65535) { bytes = 2; }
            else if (num > 65535) { bytes = 4; }
            return HexToString(num, bytes);
        }

        public static string HexToString(uint num, int bytes)
        {
            string text = string.Empty;
            if (bytes == 1)
            {
                text += HexValues[(byte)((num & 0xF0) >> 4)];
                text += HexValues[(byte)((num & 0x0F))];
            }
            else if (bytes == 2)
            {
                text += HexValues[(byte)((num & 0xF000) >> 12)];
                text += HexValues[(byte)((num & 0x0F00) >> 8)];
                text += HexValues[(byte)((num & 0x00F0) >> 4)];
                text += HexValues[(byte)((num & 0x000F))];
            }
            else if (bytes == 4)
            {
                text += HexValues[(byte)((num & 0xF0000000) >> 28)];
                text += HexValues[(byte)((num & 0x0F000000) >> 24)];
                text += HexValues[(byte)((num & 0x00F00000) >> 20)];
                text += HexValues[(byte)((num & 0x000F0000) >> 16)];
                text += HexValues[(byte)((num & 0x0000F000) >> 12)];
                text += HexValues[(byte)((num & 0x00000F00) >> 8)];
                text += HexValues[(byte)((num & 0x000000F0) >> 4)];
                text += HexValues[(byte)((num & 0x0000000F))];
            }
            return text;
        }
    }
}
